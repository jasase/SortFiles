using SortFilesPlugin.Configuration;
using SortFilesPlugin.Handler.FileContentChangers.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SortFilesPlugin.Handler.FileContentChangers.Configuration
{
    public class GpxFileContentChangerConfiguration : FileContentChangerConfiguration
    {
        public int SplitAfterMeter { get; set; }

        public GpxFileContentChangerConfiguration()
        {
            SplitAfterMeter = 1000;
        }

        public override FileContentChanger CreateHandler(IFileSortingHandlerCreator creator)
        {
            return creator.CreateHandler<GpxFileContentChanger>(this);
        }
    }
}
