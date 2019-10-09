using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationExtractors;

namespace SortFilesPlugin.Configuration
{
    public class FileInformationExtractorRegexGroupsFileNameConfiguration : FileInformationExtractorConfiguration
    {
        public string GroupRegex { get; set; }

        public override FileInformationExtractor CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileInformationExtractorRegexGroupsFileName>(this);
        }
    }
}
