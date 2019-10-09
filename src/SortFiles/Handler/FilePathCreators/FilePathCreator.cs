using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;
using SortFilesPlugin.FileInformationExtractors;
using System.IO;

namespace SortFilesPlugin.FilePathCreators
{
    public class FilePathCreator : FileSortingHandlerBase
    {
        private readonly ILogger _logger;
        private readonly FilePathCreatorConfiguration _configuration;
        private static readonly Regex Replace = new Regex(@"\$\((?<Value>[a-zA-Z]*)(:(?<Format>[a-zA-Z0-9\-_]*)){0,1}\)", RegexOptions.Compiled);

        public FilePathCreator(ILogger logger, FilePathCreatorConfiguration configuration)
            : base(configuration)
        {
            if (logger == null) throw new ArgumentNullException("logger");
            if (configuration == null) throw new ArgumentNullException("configuration");
            _logger = logger;
            _configuration = configuration;
            //TODO Check for FullPath
        }

        public FilePath CreateDestinationPath(FileInformation information, FilePath sourcePath)
        {
            PrintingAvailibleInformations(information, sourcePath);

            var result = Replace.Replace(_configuration.PathRule, delegate (Match match)
            {
                var group1 = match.Groups["Value"];
                var group2 = match.Groups["Format"];
                if (group1.Success)
                {
                    var varName = group1.Value;
                    if (information.ContainsKey(varName))
                    {
                        var value = information[varName];

                        if (group2.Success)
                        {
                            var formater = group2.Value;
                            var formatingString = "{0:" + formater + "}";
                            var newValue = string.Format(formatingString, value);
                            _logger.Debug("Substitude variable '{0}' with '{1}'", varName, newValue);
                            return newValue;
                        }
                        else
                        {
                            var newValue = value.ToString();
                            _logger.Debug("Substitude variable '{0}' with '{1}'", varName, newValue);
                            return newValue;
                        }
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException(string.Format("No value for variable '{0}' availible", varName));
                    }
                }
                throw new InvalidOperationException();
            });

            var destinationPath = new FilePath(_configuration.DestinationBasePath, Path.Combine(_configuration.DestinationBasePath, result), sourcePath.FileFinder);

            if (CheckForDuplicates(destinationPath.FullPath, sourcePath))
            {
                _logger.Debug("A file at new path {0} already exists", destinationPath.FullPath);

                var newFilePath = new FilePath(_configuration.DestinationBasePath, Path.Combine(_configuration.DestinationBasePath, "Duplicate", result), sourcePath.FileFinder);
                var counter = 1;
                while (CheckForDuplicates(newFilePath.FullPath, sourcePath))
                {
                    newFilePath = new FilePath(_configuration.DestinationBasePath, Path.Combine(_configuration.DestinationBasePath, "Duplicate", result + "_" + counter++), sourcePath.FileFinder);
                }
                return newFilePath;
            }

            return destinationPath;
        }

        private bool CheckForDuplicates(string newPath, FilePath filePath)
        {
            _logger.Debug("Checking file at path {0} already exists", newPath);
            return File.Exists(newPath);
        }

        private void PrintingAvailibleInformations(IEnumerable<KeyValuePair<string, object>> file, FilePath filePath)
        {
            var newStr = file.Select(x => string.Format("\t{0}: {1}", x.Key, x.Value));
            _logger.Info("Availible informations for file '{0}' are: {2}{1}",
                filePath.FullPath,
                string.Join(Environment.NewLine, newStr),
                Environment.NewLine);
        }
    }
}

