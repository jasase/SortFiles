using SortFilesPlugin.FileMovers;
using System.Xml.Serialization;
namespace SortFilesPlugin.Configuration
{
    public class FileMoverConfiguration : FileSortingConfigurationBase<FileMover>
    {
        public override FileMover CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileMover>(this);
        }
    }
}