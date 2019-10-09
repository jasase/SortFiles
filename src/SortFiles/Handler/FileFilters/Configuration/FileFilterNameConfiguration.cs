using SortFilesPlugin.FileFilters;
namespace SortFilesPlugin.Configuration
{
    public class FileFilterNameConfiguration : FileFilterConfiguration
    {
        public string FilterRegex { get; set; }

        public override FileFilter CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileFilterName>(this);
        }
    }
}