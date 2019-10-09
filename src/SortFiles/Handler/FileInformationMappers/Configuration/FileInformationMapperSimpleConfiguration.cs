using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.Handler.FileInformationMappers.Configuration
{
    public class FileInformationMapperSimpleConfiguration 
        : FileInformationMapperConfiguration
    {
        public FileInformationMapperSimpleKeyMappingGroup[] Mappings { get; set; }



        public override FileInformationMapper CreateHandler(IFileSortingHandlerCreator creator)
        {
            return new FileInformationMapperSimple(this);
        }
    }

    public class FileInformationMapperSimpleKeyMappingGroup
    {
        public string Key { get; set; }

        public FileInformationMapperSimpleKeyMapping[] Map { get; set; }
    }

    public class FileInformationMapperSimpleKeyMapping
    {
        public string Source { get; set; }
        public string Result { get; set; }
    }
}

