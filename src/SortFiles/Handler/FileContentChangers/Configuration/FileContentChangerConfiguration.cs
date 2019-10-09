using SortFilesPlugin.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SortFilesPlugin.Handler.FileContentChangers.Configuration
{
    [XmlInclude(typeof(GpxFileContentChangerConfiguration))]    
    public abstract class FileContentChangerConfiguration : FileSortingConfigurationBase<FileContentChanger>
    { }
}
