using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.Handler.FileInformationFilters.Configuration
{
    public class FileInformationFilterKeyExistsConfiguration : FileInformationFilterConfiguration
    {
        public string Key { get; set; }

        public override FileInformationFilter CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileInformationFilterKeyExists>(this);
        }
    }
}
