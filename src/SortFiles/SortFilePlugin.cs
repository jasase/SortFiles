using Framework.Abstraction.Extension;
using Framework.Abstraction.Plugins;
using ServiceHost.Contracts;
using Framework.Abstraction.Services.XmlToObject;
using Framework.Abstraction.IocContainer;
using System.Collections.Generic;
using GPX.Common;

namespace SortFilesPlugin
{
    public class SortFilePlugin : Plugin, IServicePlugin
    {
        private readonly PluginDescription _description;
        private FileSortingExecutor _executor;

        public SortFilePlugin(IDependencyResolver resolver, IDependencyResolverConfigurator configurator, IEventService eventService, ILogger logger)
            : base(resolver, configurator, eventService, logger)
        {
            _description = new AutostartServicePluginDescription
            {
                Name = "FileSortingPlugin",
                NeededServices = new[] { typeof(IXmlToObject),
                                         typeof(IGpxDataAccess),
                                         //typeof(IEnvironmentParameters)
                }
            };
        }

        public override PluginDescription Description
        {
            get { return _description; }
        }

        protected override void ActivateInternal()
        {
            ApplyRegistrations(CreateRegistrations());

            var factory = Resolver.GetInstance<FileSortingFactory>();
            _executor = Resolver.GetInstance<FileSortingExecutor>();

            var sorting = factory.LoadConfig();
            foreach (var item in sorting)
            {
                _executor.AddFileSorting(item);
            }
            _executor.Start();
        }

        private IEnumerable<DependencyResolverRegistration> CreateRegistrations()
        {
            yield return new SingletonRegistration<FileSortingFactory, FileSortingFactory>();
            yield return new SingletonRegistration<FileSortingExecutor, FileSortingExecutor>();
        }
    }
}
