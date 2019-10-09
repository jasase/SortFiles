using SortFilesPlugin.Configuration;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SortFilesPlugin.FileInformationExtractors
{
    public class FileInformationExtractorExifInformation : FileInformationExtractor
    {
        public FileInformationExtractorExifInformation(FileInformationExtractorExifInformationConfiguration configuration)
            : base(configuration)
        { }

        public override FileInformation ExtractInformation(FilePath file)
        {
            var information = new FileInformation();
            var theImage = new Bitmap(file.FullPath);

            PropertyItem[] propItems = theImage.PropertyItems;

            var date = propItems.FirstOrDefault(x => x.Id == 0x9003);

            var longt = propItems.FirstOrDefault(x => x.Id == 0x0004);
            var lat = propItems.FirstOrDefault(x => x.Id == 0x0002);

            if (date != null)
            {
                var encoding = new System.Text.ASCIIEncoding();
                string text = encoding.GetString(date.Value, 0, date.Len - 1);

                CultureInfo provider = CultureInfo.InvariantCulture;
                DateTime dateCreated;
                if (DateTime.TryParseExact(text, "yyyy:MM:d H:m:s", provider, DateTimeStyles.AssumeLocal, out dateCreated))
                {
                    information.AddInformation(FileInformationNames.PictureCreationDate, dateCreated);
                }
            }
            else
            {
                var fileInfo = new FileInfo(file.FullPath);
                var fileDate = fileInfo.LastWriteTime < fileInfo.CreationTime ? fileInfo.LastWriteTime 
                                                                              : fileInfo.CreationTime;
                information.AddInformation(FileInformationNames.PictureCreationDate, fileDate);
            }

            theImage.Dispose();
            return information;
        }
    }
}