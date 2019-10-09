using System;
using SortFilesPlugin.FileFilters;
using SortFilesPlugin.FileFinders;
using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.FileMovers;
using SortFilesPlugin.FilePathCreators;
using SortFilesPlugin.Handler.FileContentChangers;
using SortFilesPlugin.Handler.FileIndexers;
using SortFilesPlugin.Handler.FileInformationFilters;
using SortFilesPlugin.Handler.FileInformationMappers;

namespace SortFilesPlugin
{
    public class FileSorting
    {
        public virtual FileFinder[] FileFinder { get; private set; }
        public virtual FileFilter[] FileFilter { get; private set; }
        public virtual FileInformationExtractor[] FileInformationExtractor { get; private set; }
        public virtual FileInformationMapper[] FileInformationMapper { get; private set; }
        public virtual FileInformationFilter[] FileInformationFilter { get; private set; }
        public virtual FilePathCreator FilePathCreator { get; private set; }
        public virtual FilePathCreator BackupFilePathCreator { get; private set; }
        public virtual FileMover FileMover { get; private set; }
        public virtual FileContentChanger[] FileContentChanger { get; private set; }
        public virtual FileIndexer[] FileIndexer { get; private set; }
        public virtual int Priority { get; private set; }

        public FileSorting(int priority,
                           FileFinder[] fileFinder,
                           FileFilter[] fileFilter,
                           FileInformationExtractor[] fileInformationExtractor,
                           FileInformationMapper[] fileInformationMapper,
                           FileInformationFilter[] fileInformationFilter,
                           FilePathCreator filePathCreator,
                           FilePathCreator backupFilePathCreator,
                           FileMover fileMover,
                           FileContentChanger[] fileContentChanger,
                           FileIndexer[] fileIndexer)
        {
            Priority = priority;
            FileFinder = fileFinder ?? throw new ArgumentNullException("fileFinder");
            FileFilter = fileFilter ?? throw new ArgumentNullException("fileFilter");
            FileInformationExtractor = fileInformationExtractor ?? throw new ArgumentNullException("fileInformationExtractor");
            FileInformationMapper = fileInformationMapper ?? throw new ArgumentNullException(nameof(fileInformationMapper));
            FileInformationFilter = fileInformationFilter ?? throw new ArgumentNullException("fileInformationFilter");
            FilePathCreator = filePathCreator ?? throw new ArgumentNullException("filePathCreator");
            BackupFilePathCreator = backupFilePathCreator;
            FileMover = fileMover ?? throw new ArgumentNullException("fileMover");
            FileContentChanger = fileContentChanger ?? throw new ArgumentNullException("fileConentChanger");
            FileIndexer = fileIndexer ?? throw new ArgumentNullException("indexer");

            if (fileFinder.Length == 0) throw new ArgumentException("Minimal one fileFinder must be availible");
            if (fileInformationExtractor.Length == 0) throw new ArgumentException("Minimal one FileInformationExtractor must be availible");
        }
    }
}

