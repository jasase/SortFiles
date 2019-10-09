using SortFilesPlugin.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesPlugin
{
    public abstract class FileSortingHandlerBase
    {
        private readonly FileSortingConfigurationBase _configuration;
        public string Name { get { return _configuration.Name; } }        

        public FileSortingHandlerBase(FileSortingConfigurationBase configuration)
        {
            if (configuration == null) throw new ArgumentNullException();
            _configuration = configuration;
        }
    }
}
