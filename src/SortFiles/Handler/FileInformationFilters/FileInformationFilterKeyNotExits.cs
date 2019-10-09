using SortFilesPlugin.Handler.FileInformationFilters.Configuration;
using SortFilesPlugin.FileInformationExtractors;

namespace SortFilesPlugin.Handler.FileInformationFilters
{
    public class FileInformationFilterKeyNotExists : FileInformationFilter
    {
        private readonly FileInformationFilterKeyNotExistsConfiguration _configuration;

        public FileInformationFilterKeyNotExists(FileInformationFilterKeyNotExistsConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public override bool MatchFile(FilePath file, FileInformation information)
        {
            return !information.ContainsKey(_configuration.Key);
        }
    }
}
