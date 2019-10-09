using System;
using System.ComponentModel;
using System.IO;
using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.FileMovers
{
    public class FileMover : FileSortingHandlerBase
    {
        private readonly ILogger _logger;
        public FileMover(FileMoverConfiguration configuration, ILogger logger)
            : base(configuration)
        {
            _logger = logger;
        }

        public void ProcessFileSort(FilePath source, FilePath destination)
        {
            Process(source, destination, (f, d) => f.MoveTo(d));
        }

        public void ProcessFileBackup(FilePath source, FilePath destination)
        {
            Process(source, destination, (f, d) => f.CopyTo(d));
        }

        public void Process(FilePath source, FilePath destination, Action<FileInfo, string> fileAction)
        {
            var file = new FileInfo(source.FullPath);
            if (file.Exists)
            {
                var path = Path.GetDirectoryName(destination.FullPath);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                _logger.Info("Processing file from '{0}' to '{1}'", source.FullPath, destination.FullPath);
                fileAction(file, destination.FullPath);                
                return;
            }

            throw new InvalidOperationException("File during operation of service was removed from original location");
        }
    }
}

