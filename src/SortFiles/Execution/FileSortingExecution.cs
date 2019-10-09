using SortFilesPlugin.FileInformationExtractors;
using System;

namespace SortFilesPlugin.Execution
{
    public class FileSortingExecution
    {
        public FilePath SourceFile { get; private set; }
        public FileInformation FileInformation { get; set; }
        public FilePath DestinationPath { get; set; }
        public FilePath BackupPath { get; set; }
        public string MatchingFileFilterName { get; set; }
        public string MatchingFileInformationFilterName { get; set; }        
        public Exception Error { get; set; }
        public bool FileWasProcessed { get; internal set; }

        public FileSortingExecution(FilePath filePath)
        {
            SourceFile = filePath;
            MatchingFileInformationFilterName = null;
            MatchingFileFilterName = null;
        }
    }
}
