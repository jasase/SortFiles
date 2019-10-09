using Framework.Abstraction.Extension;
using GPX.Common;
using SortFilesPlugin.Handler.FileContentChangers.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortFilesPlugin.Handler.FileContentChangers
{
    public class GpxFileContentChanger : FileContentChanger
    {
        private readonly GpxFileContentChangerConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IGpxDataAccess _gpxDataAccess;

        public GpxFileContentChanger(GpxFileContentChangerConfiguration configuration, IGpxDataAccess gpxDataAccess, ILogger logger)
            : base(configuration)
        {
            _logger = logger;
            _gpxDataAccess = gpxDataAccess;
            _configuration = configuration;
        }

        public override void ChangeContent(FilePath newFile)
        {
            var file = new FileInfo(newFile.FullPath);
            if (file.Exists)
            {
                var reader = file.OpenText();

                var content = reader.ReadToEnd();
                var gpxFile = _gpxDataAccess.Read(content);

                
                var newTracks = new List<GpxTrack>();
                foreach (var currentTrack in gpxFile.Tracks)
                {
                    
                    var points = currentTrack.Segments.SelectMany(x => x.TrackPoints).ToArray();
                    var distance = new double[points.Length - 1];
                    var time = new TimeSpan[points.Length - 1];
                    var speed = new double[points.Length - 1];
                    var newTrack = CreateNewTrack(currentTrack);

                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        distance[i] = points[i].Coordinate.DistanceTo(points[i + 1].Coordinate);
                        time[i] = points[i+1].Time.Subtract(points[i].Time);
                        speed[i] = (distance[i] / 1000) / time[i].TotalHours;
                        var conditionCounter = 0;
                        if (distance[i] > 500)
                        {
                            conditionCounter++;
                        }
                        if (time[i] > TimeSpan.FromMinutes(5))
                        {
                            conditionCounter++;
                        }
                        if (speed[i] > 1500)
                        {
                            conditionCounter++;
                        }

                        newTrack.Segments[0].TrackPoints.Add(points[i]);

                        if (conditionCounter >= 2)
                        {
                            newTracks.Add(newTrack);
                            newTrack = CreateNewTrack(currentTrack);                                                
                        }                        
                    }                                      

                    newTracks.Add(newTrack);
                }

                gpxFile.Tracks.Clear();
                foreach (var track in newTracks)
                {
                    var pointCountArray = track.Segments.SelectMany(x => x.TrackPoints).ToArray();
                    var pointCount = pointCountArray.Length;

                    if (pointCount < 10)
                    {
                        continue;
                    }
                    if (pointCount < 100 && !ReachedDistance(pointCountArray))
                    {
                        continue;
                    }

                    gpxFile.Tracks.Add(track);
                }
                

                var newContent = _gpxDataAccess.Write(gpxFile);
                reader.Close();
                reader.Dispose();

                File.WriteAllText(newFile.FullPath, newContent);

                return;
            }

            throw new InvalidOperationException("File during operation of service was removed from original location");
        }

        private bool ReachedDistance(GpxWaypoint[] pointCountArray)
        {
            for (int i1 = 0; i1 < pointCountArray.Length; i1++)
            {
                for (int i2 = 0; i2 < pointCountArray.Length; i2++)
                {
                    var disctane = pointCountArray[i1].Coordinate.DistanceTo(pointCountArray[i2].Coordinate);
                    if (disctane > 100)
                    {
                        return true;
                    }
                }                
            }

            return false;
        }

        private GpxTrack CreateNewTrack(GpxTrack track)
        {
            var newTrack = new GpxTrack()
            {
                Cmt = track.Cmt,
                Description = track.Description,
                Extensions = track.Extensions,
                Name = track.Name,
                Number = track.Number,
                Source = track.Source,
                Type = track.Type
            };
            newTrack.Segments.Add(new GpxTrackSegment());
            return newTrack;
        }
    }
}
