using SortFilesPlugin.FileFinders;
namespace SortFilesPlugin.Configuration
{
    public class FileFinderConfiguration : FileSortingConfigurationBase<FileFinder>
    {
        public string SearchingPath { get; set; }

        public bool DeleteEmptyFolders { get; set; }

        public bool Recursive { get; set; }

        public override FileFinder CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileFinder>(this);
        }
    }
}