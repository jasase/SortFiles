using SortFilesPlugin.FileInformationExtractors;
namespace SortFilesPlugin.Configuration
{
    public class FileInformationExtractorExifInformationConfiguration : FileInformationExtractorConfiguration
    {
        public override FileInformationExtractors.FileInformationExtractor CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileInformationExtractorExifInformation>(this);
        }
    }
}