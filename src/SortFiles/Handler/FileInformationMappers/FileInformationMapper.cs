using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationMappers.Configuration;

namespace SortFilesPlugin.Handler.FileInformationMappers
{
    public abstract class FileInformationMapper : FileSortingHandlerBase
    {
        public FileInformationMapper(FileInformationMapperConfiguration configuration)
            : base(configuration)
        { }

        public abstract FileInformation MapInformation(FileInformation information);
    }
}
