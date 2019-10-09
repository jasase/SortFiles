using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;
using SortFilesPlugin.FileInformationExtractors;
using System.Text.RegularExpressions;
using System.Linq;

namespace SortFilesPlugin.Handler.FileInformationExtractors
{
    public class FileInformationExtractorRegexGroupsFileName : FileInformationExtractor
    {
        private readonly ILogger _logger;
        private readonly FileInformationExtractorRegexGroupsFileNameConfiguration _configuration;
        private readonly Regex _regex;

        public FileInformationExtractorRegexGroupsFileName(ILogger logger,
                                                           FileInformationExtractorRegexGroupsFileNameConfiguration configuration)
            : base(configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _regex = new Regex(_configuration.GroupRegex);
        }

        public override FileInformation ExtractInformation(FilePath file)
        {
            var informations = new FileInformation();
            _logger.Debug("Extracting informations from filename (not Path) with regex '{1}' from file '{0}'", file.FullPath, _configuration.GroupRegex);

            var match = _regex.Match(file.FileName);
            if (match.Success)
            {
                foreach (var groupName in _regex.GetGroupNames())
                {
                    var group = match.Groups[groupName];
                    if (group.Success)
                    {
                        informations.AddInformation($"regexG{groupName}", group.Value);
                    }
                }
            }
            else
            {
                _logger.Warn("Extracting informations from filename (not Path) with regex '{1}' from file '{0}' not possible, because regex is not matching",
                             file.FullPath,
                             _configuration.GroupRegex);
            }

            return informations;
        }
    }
}
