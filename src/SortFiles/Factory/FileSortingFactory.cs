using System;
using System.Collections.Generic;
using System.Linq;
using SortFilesPlugin.Configuration;
using SortFilesPlugin.FilePathCreators;
using StructureMap;
using System.IO;
using Framework.Abstraction.Services.XmlToObject;
using Framework.Abstraction.Extension;
using Framework.Abstraction.Services;
using System.Diagnostics;

namespace SortFilesPlugin
{
    public class FileSortingFactory : IFileSortingHandlerCreator
    {
        private readonly IXmlToObject _xmlToObject;
        private readonly IContainer _container;
        private readonly ILogger _logger;
        private readonly IEnvironmentParameters _parameter;

        public FileSortingFactory(IXmlToObject xmlToObject, IContainer container, ILogger logger, IEnvironmentParameters parameter)
        {
            _logger = logger;
            _container = container;
            _xmlToObject = xmlToObject;
            _parameter = parameter;
        }

        public IEnumerable<FileSorting> LoadConfig()
        {
            Debugger.Launch();
            var configPath = new DirectoryInfo(Path.Combine(_parameter.ConfigurationDirectory.FullName, "FileSorting"));

            if (configPath.Exists)
            {
                foreach (var configFile in configPath.GetFiles("*.xml"))
                {
                    _logger.Info("Loading file sorting configuration from path '{0}'", configFile.FullName);
                    var config = _xmlToObject.ReadXml<FileSortingConfiguration>(configFile.FullName);

                    foreach (var rule in config.Rules)
                    {
                        yield return CreateSorting(rule, config);
                    }
                }
            }
            else
            {
                _logger.Error("No file sorting configuration with extensio *.xml at path '{0}' found. Please create a configuration", configPath);
            }

        }

        private FileSorting CreateSorting(FileSortingRule ruleToCreate, FileSortingConfiguration configuration)
        {
            var finder = LoadHandler(ruleToCreate.FinderNames, configuration.Finder);
            var filter = LoadHandler(ruleToCreate.FilterNames, configuration.Filter);
            var informationExtractor = LoadHandler(ruleToCreate.InformationExtractorNames, configuration.InformationExtractor);
            var informationMapper = LoadHandler(ruleToCreate.InformationMapperNames, configuration.InformationMapper);
            var fileInformationFilter = LoadHandler(ruleToCreate.InformationFilterNames, configuration.InformationFilter);
            var pathCreator = LoadHandler(ruleToCreate.PathCreatorName, configuration.Creator);
            FilePathCreator backupPathCreator = null;
            if (!string.IsNullOrEmpty(ruleToCreate.BackupPathCreatorName))
            {
                backupPathCreator = LoadHandler(ruleToCreate.BackupPathCreatorName, configuration.Creator);
            }
            var mover = LoadHandler(ruleToCreate.MoverName, configuration.Mover);
            var changer = LoadHandler(ruleToCreate.FileContentChanger, configuration.ContentChanger);
            var indexer = LoadHandler(ruleToCreate.Indexer, configuration.Indexer);

            return new FileSorting(ruleToCreate.Priority,
                finder.ToArray(),
                filter.ToArray(),
                informationExtractor.ToArray(),
                informationMapper.ToArray(),
                fileInformationFilter.ToArray(),
                pathCreator,
                backupPathCreator,
                mover,
                changer.ToArray(),
                indexer.ToArray());
        }

        private IEnumerable<T> LoadHandler<T>(string[] filterNamen, FileSortingConfigurationBase<T>[] configurations)
            where T : FileSortingHandlerBase
        {
            configurations = configurations ?? new FileSortingConfigurationBase<T>[0];
            filterNamen = filterNamen ?? new string[0];
            var matchingConfigs = configurations.Where(x => filterNamen.Contains(x.Name)).ToArray();
            if (filterNamen.Length != matchingConfigs.Length)
            {
                var missingHandler = filterNamen.Except(matchingConfigs.Select(x => x.Name));
                throw new ArgumentException("Config contains not existing handler of type '" + typeof(T).Name + "'. Missing handlers are " + string.Join(", ", missingHandler) +
                                            "\r\nExisting Handlers are: " + string.Join(", ", configurations.Select(x => x.Name)));
            }

            foreach (var config in matchingConfigs)
            {
                yield return config.CreateHandler(this);
            }
        }

        private T LoadHandler<T>(string filterName, FileSortingConfigurationBase<T>[] configurations)
            where T : FileSortingHandlerBase
        {
            configurations = configurations ?? new FileSortingConfigurationBase<T>[0];
            var matchingConfig = configurations.Where(x => (x.Name ?? string.Empty).Equals(filterName)).FirstOrDefault();
            if (matchingConfig == null)
            {
                throw new ArgumentException("Config contains not existing handler of type '" + typeof(T).Name + "'. Missing handlers is " + filterName);
            }

            return matchingConfig.CreateHandler(this);
        }

        public T CreateHandler<T>(FileSortingConfigurationBase config) where T : FileSortingHandlerBase
        {
            return _container.With(config.GetType(), config).GetInstance<T>();
        }
    }
}
