using SortFilesPlugin.Handler.FileContentChangers.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesPlugin.Handler.FileContentChangers
{
    public abstract class FileContentChanger : FileSortingHandlerBase
    {
        public FileContentChanger(FileContentChangerConfiguration configuration)
            : base(configuration)
        { }

        public abstract void ChangeContent(FilePath newFile);
    }
}
