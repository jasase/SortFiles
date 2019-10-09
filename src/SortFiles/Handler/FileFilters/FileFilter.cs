using SortFilesPlugin.Configuration;
namespace SortFilesPlugin.FileFilters
{
    public abstract class FileFilter : FileSortingHandlerBase
    {
        public abstract bool MatchFile(FilePath file);

        public FileFilter(FileFilterConfiguration configuration)
            : base(configuration)
        { }
    }
}

