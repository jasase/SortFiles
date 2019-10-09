using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace SortFilesPlugin.Exif
{
    public static class ExifHelper
    {
        public static PropertyItem CreatePropertyItem(short type, int tag, int len, byte[] value)
        {
            PropertyItem item;

            // Loads a PropertyItem from a Jpeg image stored in the assembly as a resource.
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream emptyBitmapStream = assembly.GetManifestResourceStream("SortFilesPlugin.Exif.decoy.jpg");
            System.Drawing.Image empty = System.Drawing.Image.FromStream(emptyBitmapStream);

            item = empty.PropertyItems[0];

            // Copies the data to the property item.
            item.Type = type;
            item.Len = len;
            item.Id = tag;
            item.Value = new byte[value.Length];
            value.CopyTo(item.Value, 0);

            return item;
        }
    }
}
