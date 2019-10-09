using SortFilesPlugin.FileInformationExtractors;
namespace SortFilesPlugin.Configuration
{
    public class FileInformationExtractorFileNameConfiguration : FileInformationExtractorConfiguration
    {
        public override FileInformationExtractor CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileInformationExtractorFileName>(this);
        }
    }
}