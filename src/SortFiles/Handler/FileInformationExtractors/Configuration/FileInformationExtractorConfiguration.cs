using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationExtractors;
using System.Xml.Serialization;
namespace SortFilesPlugin.Configuration
{
    [XmlInclude(typeof(FileInformationExtractorExifInformationConfiguration))]
    [XmlInclude(typeof(FileInformationExtractorGpxInformationConfiguration))]
    [XmlInclude(typeof(FileInformationExtractorFileNameConfiguration))]
    [XmlInclude(typeof(FileInformationExtractorRegexGroupsFileNameConfiguration))]
    public abstract class FileInformationExtractorConfiguration : FileSortingConfigurationBase<FileInformationExtractor>
    {
    }
}
