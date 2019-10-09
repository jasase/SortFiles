using SortFilesPlugin.Configuration;
using System.Xml.Serialization;

namespace SortFilesPlugin.Handler.FileInformationMappers.Configuration
{
    [XmlInclude(typeof(FileInformationMapperSimpleConfiguration))]
    //[XmlInclude(typeof(FileInformationFilterKeyNotExistsConfiguration))]
    public abstract class FileInformationMapperConfiguration
        : FileSortingConfigurationBase<FileInformationMapper>
    { }
}
