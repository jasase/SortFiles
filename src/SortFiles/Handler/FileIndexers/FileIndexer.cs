using SortFilesPlugin.Handler.FileIndexers.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesPlugin.Handler.FileIndexers
{
    public abstract class FileIndexer : FileSortingHandlerBase
    {
        public FileIndexer(FileIndexerConfiguration configuration)
            : base(configuration)
        { }

        public abstract void IndexFile(FilePath newFilePath);
    }
}
