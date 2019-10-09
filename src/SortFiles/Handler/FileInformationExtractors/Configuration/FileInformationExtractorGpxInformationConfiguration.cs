using SortFilesPlugin.Configuration;
using SortFilesPlugin.FileInformationExtractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortFilesPlugin.Handler.FileInformationExtractors
{
    public class FileInformationExtractorGpxInformationConfiguration : FileInformationExtractorConfiguration
    {
        public override FileInformationExtractor CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<FileInformationExtractorGpxInformation>(this);
        }
    }
}
