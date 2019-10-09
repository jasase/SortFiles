using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Framework.Abstraction.Extension;
using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Execution;

namespace SortFilesPlugin
{
    public class FileSortingExecutor
    {
        private const string MoveLogFileName = "MoveLog.log";

        private readonly ILogger _logger;
        private readonly List<FileSorting> _fileSortings;
        private readonly Timer _timer;

        private bool _executing;
        private object _timerLock;

        public IEnumerable<FileSorting> ActiveFileSortings { get { return _fileSortings; } }
        public int ExecutionCounter { get; private set; }

        public FileSortingExecutor(ILogger logger)
        {
            _logger = logger;
            _fileSortings = new List<FileSorting>();
            _executing = false;
            _timerLock = new object();
            _timer = new Timer(TimerCallback);
            ExecutionCounter = 0;

        }

        private void TimerCallback(object state)
        {
            var startTime = DateTime.Now;
            _logger.Info("Begin with FileSorting cycle at {0}", startTime);
            try
            {
                lock (_timerLock)
                {
                    if (_executing)
                    {
                        _logger.Info("FileSorting cycle stopped, because annother cycle is running.");
                        return;
                    }
                    _executing = true;
                }

                foreach (var fileSorter in _fileSortings.OrderBy(x => x.Priority))
                {
                    try
                    {
                        Execute(fileSorter);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "Error occured during executing file sorting");
                    }
                }
            }
            finally
            {
                lock (_timerLock)
                {
                    _executing = false;
                }

                ExecutionCounter++;
                var finsihTime = DateTime.Now;
                _logger.Debug("Completed FileSorting cycle that was started at {0}. Execution time was {1}", startTime, finsihTime - startTime);
            }
        }

        private void Execute(FileSorting sorting)
        {
            _logger.Debug("Executing next FileSorting");
            var files = sorting.FileFinder.SelectMany(s =>
            {
                return s.FindFiles().Select(f => new FileSortingExecution(f));
            });

            var options = new ParallelOptions { MaxDegreeOfParallelism = 1 };
            var results = new List<FileSortingExecution>();
            var loop = Parallel.ForEach(files, options, x =>
            {
                try
                {
                    Execute(x, sorting);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Moving of file {0} not possible due to:", x.SourceFile.FullPath);
                    x.Error = ex;
                }
                results.Add(x);
            });

            foreach (var entry in results)
            {
                var logPath = Path.Combine(entry.SourceFile.BasePath, MoveLogFileName);
                File.AppendAllText(logPath, DumpResult(entry));
            }

            foreach (var finder in sorting.FileFinder)
            {
                finder.RemoveEmptyFolders();
            }

            _logger.Debug("Finsihed executing of FileSorting");
        }

        private void Execute(FileSortingExecution execution, FileSorting sorting)
        {
            _logger.Debug("Executing file filter");
            var fileFilterWasMatching = true;
            foreach (var filter in sorting.FileFilter)
            {
                if (filter.MatchFile(execution.SourceFile))
                {
                    execution.MatchingFileFilterName = filter.Name ?? string.Empty;
                    fileFilterWasMatching = true;
                    break;
                }
                fileFilterWasMatching = false;
            }

            if (!fileFilterWasMatching)
            {
                _logger.Debug("No Filter matching for file {0}", execution.SourceFile.FullPath);
                execution.FileWasProcessed = false;
                return;
            }

            execution.FileWasProcessed = true;

            _logger.Debug("Extracting informations from file {0}", execution.SourceFile.FullPath);
            var fileInformations = sorting.FileInformationExtractor.Select(x => x.ExtractInformation(execution.SourceFile));
            execution.FileInformation = MergeFileInformation(fileInformations);

            _logger.Debug("Mapping informations from file {0}", execution.SourceFile.FullPath);
            foreach (var mapper in sorting.FileInformationMapper)
            {
                execution.FileInformation = mapper.MapInformation(execution.FileInformation);
            }

            _logger.Debug("Executing fileInformation filter");
            var fileInformationFilterWasMatching = true;
            foreach (var filter in sorting.FileInformationFilter)
            {
                if (filter.MatchFile(execution.SourceFile, execution.FileInformation))
                {
                    execution.MatchingFileInformationFilterName = filter.Name ?? string.Empty;
                    fileInformationFilterWasMatching = true;
                    break;
                }
                fileInformationFilterWasMatching = false;
            }

            if (!fileInformationFilterWasMatching)
            {
                _logger.Debug("No fileInformation Filter matching for file {0}", execution.SourceFile.FullPath);
                return;
            }

            _logger.Debug("Creating new file path for file {0}", execution.SourceFile.FullPath);
            execution.DestinationPath = sorting.FilePathCreator.CreateDestinationPath(execution.FileInformation, execution.SourceFile);

            if (sorting.BackupFilePathCreator != null)
            {
                _logger.Debug("Creating backup file path for file {0}", execution.SourceFile.FullPath);
                execution.BackupPath = sorting.BackupFilePathCreator.CreateDestinationPath(execution.FileInformation, execution.SourceFile);

                _logger.Debug("Backuping file {0}", execution.SourceFile.FullPath);
                sorting.FileMover.ProcessFileBackup(execution.SourceFile, execution.BackupPath);
            }

            _logger.Debug("Moving file {0}", execution.SourceFile.FullPath);
            sorting.FileMover.ProcessFileSort(execution.SourceFile, execution.DestinationPath);

            _logger.Debug("Change content of file {0}", execution.DestinationPath.FullPath);
            foreach (var contentChanger in sorting.FileContentChanger)
            {
                contentChanger.ChangeContent(execution.DestinationPath);
            }

            _logger.Debug("Indexing file {0}", execution.DestinationPath.FullPath);
            foreach (var indexer in sorting.FileIndexer)
            {
                indexer.IndexFile(execution.DestinationPath);
            }
        }

        private FileInformation MergeFileInformation(IEnumerable<FileInformation> fileInformations)
        {
            var informations = new FileInformation();

            foreach (var oldInfo in fileInformations.SelectMany(x => x))
            {
                if (informations.ContainsKey(oldInfo.Key))
                {
                    _logger.Warn("Overiding already existing key '{0}'", oldInfo.Key);
                }
                informations[oldInfo.Key] = oldInfo.Value;
            }

            return informations;
        }

        private string DumpResult(FileSortingExecution entry)
        {
            const string format = "{0,-80} | {1}";
            const string formatSubInformations = "{0,80} | {1}";

            var sb = new StringBuilder();

            if (entry.FileWasProcessed)
            {
                sb.AppendFormat(format, entry.SourceFile.FullPath, entry.DestinationPath != null ? entry.DestinationPath.FullPath : string.Empty);
                sb.AppendLine();
                if (entry.MatchingFileFilterName != null)
                {
                    sb.AppendFormat(formatSubInformations, "Matching FileFilter", entry.MatchingFileInformationFilterName);
                    sb.AppendLine();
                }
                if (entry.FileInformation != null)
                {
                    foreach (var info in entry.FileInformation)
                    {
                        sb.AppendFormat(formatSubInformations, info.Key, info.Value);
                        sb.AppendLine();
                    }
                }
                if (entry.MatchingFileInformationFilterName != null)
                {
                    sb.AppendFormat(formatSubInformations, "Matching FileInformationFilter", entry.MatchingFileInformationFilterName);
                    sb.AppendLine();
                }
                if (entry.Error != null)
                {
                    sb.AppendFormat(formatSubInformations, "Error", entry.Error.ToString());
                    sb.AppendLine();
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public void AddFileSorting(FileSorting fileSorting)
        {
            _fileSortings.Add(fileSorting);
        }

        public void Start()
        {
            _logger.Info("Starting FileSorting execution");
            _timer.Change(1, Convert.ToInt32(TimeSpan.FromMinutes(5).TotalMilliseconds));
        }

        public void Stop()
        {
            _logger.Info("Stoping FileSorting execution");
            _timer.Change(-1, -1);
        }
    }
}
