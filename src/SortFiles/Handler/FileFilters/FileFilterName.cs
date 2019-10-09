using System;
using System.Text.RegularExpressions;
using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.FileFilters
{
    public class FileFilterName : FileFilter
    {
        private readonly ILogger _logger;
        private readonly FileFilterNameConfiguration _configuration;
        private readonly Regex _filter;

        public FileFilterName(ILogger logger, FileFilterNameConfiguration configuration)
            : base(configuration)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (configuration == null) throw new ArgumentNullException("configuration");
            _logger = logger;
            _configuration = configuration;

            _filter = new Regex(_configuration.FilterRegex, RegexOptions.Compiled);
        }

        public override bool MatchFile(FilePath file)
        {
            var result = _filter.IsMatch(file.FileName);
            _logger.Debug("Filter '{0}' match {2} for file '{1}'", _configuration.Name, file.FullPath, result ? "" : "not");
            return result;
        }
    }
}