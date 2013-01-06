using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;


namespace WhereAmI
{
    [DataContract]
    public class Workout
    {
        [DataMember]
        public string workoutName { get; set; }
        [DataMember]
        public string startTime { get; set; }
        [DataMember]
        public double distanceRan { get; set; }
        [DataMember]
        public string workoutDuration { get; set; }
        [DataMember]
        public List<GeoCoordinate> routeCoordinates { get; set; }
        [DataMember]
        public List<Tuple<GeoCoordinate, DateTime>> route { get; set; }
        [DataMember]
        public MapPolyline routeLine = new MapPolyline();
        [DataMember]
        public static LocationCollection Locations { get; set; }
        [DataMember]
        public ObservableCollection<double> kmPerHour = new ObservableCollection<double>();
        [DataMember]
        public TimeSpan elapsedTimeTS { get; set; }
     // [DataMember]

        public Workout()
        {
            routeLine.Locations = new LocationCollection();
        }
        
        public override string ToString()
        {
            return workoutName;
        }

        internal void addKMpHtoKMpHcollection(double kilometersPerHour)
        {
            kmPerHour.Add(kilometersPerHour);
        }
    }
}
