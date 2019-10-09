using SortFilesPlugin.FileInformationExtractors;
using SortFilesPlugin.Handler.FileInformationFilters.Configuration;

namespace SortFilesPlugin.Handler.FileInformationFilters
{
    public class FileInformationFilterKeyExists : FileInformationFilter
    {
        private readonly FileInformationFilterKeyExistsConfiguration _configuration;

        public FileInformationFilterKeyExists(FileInformationFilterKeyExistsConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public override bool MatchFile(FilePath file, FileInformation information)
        {
            return information.ContainsKey(_configuration.Key);
        }
    }
}
