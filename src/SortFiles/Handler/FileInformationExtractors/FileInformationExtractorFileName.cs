using System.IO;
using Framework.Abstraction.Extension;
using SortFilesPlugin.Configuration;

namespace SortFilesPlugin.FileInformationExtractors
{
    public class FileInformationExtractorFileName : FileInformationExtractor
    {
        private readonly ILogger _logger;

        public FileInformationExtractorFileName(ILogger logger, FileInformationExtractorFileNameConfiguration configuration)
            : base(configuration)
        {
            _logger = logger;
        }

        public override FileInformation ExtractInformation(FilePath file)
        {
            var informations = new FileInformation();
            _logger.Debug("Extracting filename informations from file '{0}'", file.FullPath);

            var fileName = Path.GetFileNameWithoutExtension(file.FullPath);
            informations.AddInformation(FileInformationNames.FileName, fileName);

            var extension = Path.GetExtension(file.FullPath);
            informations.AddInformation(FileInformationNames.FileExtension, extension);            

            string directoryName = Path.GetDirectoryName(file.FullPath);
            if (!string.IsNullOrEmpty(directoryName))
            {
                directoryName = directoryName.Replace(file.BasePath, "").Trim(Path.DirectorySeparatorChar);
                var parentDirectories = directoryName.Split(Path.DirectorySeparatorChar);
                informations.AddInformation(FileInformationNames.ParentDirectories, parentDirectories);
            }

            var fileInfo = new FileInfo(file.FullPath);
            if(fileInfo.Exists)
            {
                informations.AddInformation(FileInformationNames.FileCreateDate, fileInfo.CreationTime);
                informations.AddInformation(FileInformationNames.FileLastChangeDate, fileInfo.LastWriteTime);
                informations.AddInformation(FileInformationNames.FileLastAccessDate, fileInfo.LastAccessTime);
            }

            return informations;
        }
    }
}