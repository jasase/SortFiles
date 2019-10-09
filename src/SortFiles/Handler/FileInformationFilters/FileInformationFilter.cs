using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationFilters.Configuration;

namespace SortFilesPlugin.Handler.FileInformationFilters
{
    public abstract class FileInformationFilter : FileSortingHandlerBase
    {
        public FileInformationFilter(FileInformationFilterConfiguration configuration) 
            : base(configuration)
        { }

        public abstract bool MatchFile(FilePath file, FileInformation information);
    }
}
