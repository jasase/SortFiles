using SortFilesPlugin.Configuration;
using System.Xml.Serialization;

namespace SortFilesPlugin.Handler.FileInformationFilters.Configuration
{
    [XmlInclude(typeof(FileInformationFilterKeyExistsConfiguration))]
    [XmlInclude(typeof(FileInformationFilterKeyNotExistsConfiguration))]
    public abstract class FileInformationFilterConfiguration : FileSortingConfigurationBase<FileInformationFilter>
    { }    
}
