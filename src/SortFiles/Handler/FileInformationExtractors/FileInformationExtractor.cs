using SortFilesPlugin.Configuration;
namespace SortFilesPlugin.FileInformationExtractors
{
    public abstract class FileInformationExtractor : FileSortingHandlerBase
    {
        public abstract FileInformation ExtractInformation(FilePath file);

        public FileInformationExtractor(FileInformationExtractorConfiguration configuration)
            : base(configuration)
        { }
    }
}

