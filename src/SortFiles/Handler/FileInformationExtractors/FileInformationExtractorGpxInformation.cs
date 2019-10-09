using System.Linq;
using SortFilesPlugin.Handler.FileInformationExtractors;
using GPX.Common;
using System.IO;

namespace SortFilesPlugin.FileInformationExtractors
{
    public class FileInformationExtractorGpxInformation : FileInformationExtractor
    {
        private readonly IGpxDataAccess _gpxDataAccess;
        public FileInformationExtractorGpxInformation(FileInformationExtractorGpxInformationConfiguration configuration,
                                                      IGpxDataAccess gpxDataAccess)
            : base(configuration)
        {
            _gpxDataAccess = gpxDataAccess;
        }

        public override FileInformation ExtractInformation(FilePath file)
        {
            var information = new FileInformation();
            var gpxFiles = _gpxDataAccess.Read(File.ReadAllText(file.FullPath));

            var date = (from t in gpxFiles.Tracks
                        from s in t.Segments
                        from p in s.TrackPoints
                        where p.TimeSpecified
                        select p.Time).ToArray();

            if (date.Any())
            {
                information.AddInformation(FileInformationNames.GpxDate, date.Min());
            }

            return information;
        }
    }
}