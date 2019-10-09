using System.IO;
using SortFilesPlugin.FileFinders;

namespace SortFilesPlugin
{
    public class FilePath
    {
        public string BasePath { get; private set; }
        public string FullPath { get; private set; }
        public FileFinder FileFinder { get; private set; }
        public string FileName { get { return Path.GetFileName(FullPath); } }

        public FilePath(string basePath, string fullPath, FileFinder fileFinder)
        {
            BasePath = basePath;
            FullPath = fullPath;
            FileFinder = fileFinder;
        }

        public override string ToString()
        {
            return FullPath ?? base.ToString();
        }
    }
}
