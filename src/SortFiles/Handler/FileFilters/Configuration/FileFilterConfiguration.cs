using SortFilesPlugin.FileFilters;
using System.Xml.Serialization;
namespace SortFilesPlugin.Configuration
{
    [XmlInclude(typeof(FileFilterNameConfiguration))]
    public abstract class FileFilterConfiguration : FileSortingConfigurationBase<FileFilter>
    { }
}