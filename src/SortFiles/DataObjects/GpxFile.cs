using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesPlugin.DataObjects
{
    public class GpxTrack
    {
        public string Name { get; set; }
        public List<GpxPoint> Points { get; private set; }

        public GpxTrack()
        {
            Points = new List<GpxPoint>();
        }
    }

    public class GpxPoint
    {
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
