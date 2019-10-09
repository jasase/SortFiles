using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.FileFinders
{
    public class FileFinder : FileSortingHandlerBase
    {
        private const string ThumsDbFilename = "Thumbs.db";

        private readonly FileFinderConfiguration _fileFinderConfiguration;
        private readonly ILogger _logger;

        public FileFinder(FileFinderConfiguration fileFinderConfiguration, ILogger logger)
            : base(fileFinderConfiguration)
        {
            if (fileFinderConfiguration == null) throw new ArgumentNullException("fileFinderConfiguration");
            if (logger == null) throw new ArgumentNullException("logger");

            _fileFinderConfiguration = fileFinderConfiguration;
            _logger = logger;
        }

        public virtual IEnumerable<FilePath> FindFiles()
        {
            _logger.Debug("Beginning searching for file in searching path {0}", _fileFinderConfiguration.SearchingPath);
            var dir = new DirectoryInfo(_fileFinderConfiguration.SearchingPath);
            if (!dir.Exists)
                throw new InvalidOperationException(string.Format("Searching path '{0}' does not exists",
                    _fileFinderConfiguration.SearchingPath));

            return FindFilesInFolder(dir);
        }

        public void RemoveEmptyFolders()
        {
            if (_fileFinderConfiguration.DeleteEmptyFolders)
            {
                _logger.Info("Searching for empty folders in {0}", _fileFinderConfiguration.SearchingPath);
                foreach (var folder in Directory.GetDirectories(_fileFinderConfiguration.SearchingPath))
                {
                    DeleteEmptyFolders(new DirectoryInfo(folder));
                }
            }
        }

        private bool DeleteEmptyFolders(DirectoryInfo dir)
        {
            if (!dir.GetFiles().Any(x => x.Attributes != FileAttributes.Hidden && !x.Name.Equals(ThumsDbFilename)))
            {
                if (dir.GetDirectories().All(x => DeleteEmptyFolders(x)))
                {
                    _logger.Debug("Deleting empty folder {0}", dir.FullName);
                    dir.Delete(true);
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<FilePath> FindFilesInFolder(DirectoryInfo dir)
        {
            _logger.Debug("Searching with finder '{1}' for file in folder {0}", dir.FullName, _fileFinderConfiguration.Name);
            foreach (var file in dir.EnumerateFiles())
            {
                _logger.Debug("Finder '{1}' found file {0}", file.FullName, _fileFinderConfiguration.Name);
                var filePath = new FilePath(_fileFinderConfiguration.SearchingPath, file.FullName, this);
                yield return filePath;
            }

            if (_fileFinderConfiguration.Recursive)
            {
                foreach (var subDir in dir.EnumerateDirectories())
                {
                    foreach (var subFile in FindFilesInFolder(subDir))
                    {
                        yield return subFile;
                    }
                }
            }
        }
    }
}

