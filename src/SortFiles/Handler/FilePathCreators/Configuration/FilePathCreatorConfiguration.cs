using SortFilesPlugin.FilePathCreators;
namespace SortFilesPlugin.Configuration
{
    public class FilePathCreatorConfiguration : FileSortingConfigurationBase<FilePathCreator>
    {
        public string PathRule { get; set; }

        public string DestinationBasePath { get; set; }

        public override FilePathCreator CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FilePathCreator>(this);
        }
    }
}