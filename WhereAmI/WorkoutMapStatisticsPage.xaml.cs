using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Controls.Maps;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Device.Location;

namespace WhereAmI
{
    public partial class WorkoutMapStatisticsPage : PhoneApplicationPage
    {
        MapPolyline routeLine;
        static Workout selectedWorkout = new Workout();

        public WorkoutMapStatisticsPage()
        {
            InitializeComponent();
            routeLine = new MapPolyline();

            // routeLine.Locations = new LocationCollection();
            routeLine.Locations = selectedWorkout.routeLine.Locations; // Where the magic happens: Note: ALL routes MUST have a polyline, otherwise the application WILL ofcourse, CRASH!

            System.Diagnostics.Debug.WriteLine("Done 1");
            routeLine.Stroke = new SolidColorBrush(Colors.Blue);
            System.Diagnostics.Debug.WriteLine("Done 2");
            routeLine.StrokeThickness = 5;
            routeLine.Opacity = 0.7;

            //routeLine.Locations.Add(new GeoCoordinate(34.0568, -117.195));
            //routeLine.Locations.Add(new GeoCoordinate(34.0668, -117.295));
            //routeLine.Locations.Add(new GeoCoordinate(34.0768, -117.395));
            //routeLine.Locations.Add(new GeoCoordinate(34.0868, -117.495));
            //routeLine.Locations.Add(new GeoCoordinate(34.0968, -117.595));
            //routeLine.Locations.Add(new GeoCoordinate(34.1068, -117.695));

            map2.Children.Add(routeLine); // Where the second magic happens
            textBlock5.Text = selectedWorkout.workoutName;
            textBlock6.Text = selectedWorkout.startTime;
            textBlock7.Text = selectedWorkout.distanceRan.ToString();
            textBlock8.Text = selectedWorkout.workoutDuration;
            double latitude;
            double longitude;
            getWorkoutRouteLocation(out latitude, out longitude);
            //map2.Center = new GeoCoordinate(34.0568, -117.195);
            //map2.Center = new GeoCoordinate(30.00, -100.00);
            map2.Center = new GeoCoordinate(latitude, longitude);

            // System.Diagnostics.Debug.WriteLine(routeLine.Locations.Select<routeLine.Locations, GeoCoordinate>(
            map2.ZoomLevel = 16;
   
            
        }


        public static void setWorkout(Workout selectedWorkoutItem)
        {
            selectedWorkout = selectedWorkoutItem;
            // foreach 
            //System.Diagnostics.Debug.WriteLine(workout.routeLine.Locations);
        }

        public static void getWorkoutRouteLocation(out double latitude, out double longitude)
        {
            //var allCoordinates = from Workout workout in selectedWorkout select selectedWorkout.routeLine;
           // List<GeoCoordinate> points = from point in selectedWorkout.routeLine.Locations select new GeoCoordinate(point.lat, point.lng);
            //List<GeoCoordinate> a = selectedWorkout.routeCoordinates;
            //double latitude2 = from GeoCoordinate geoCor in selectedWorkout.routeCoordinates.First() select geoCor.Latitude;
            Tuple<GeoCoordinate, DateTime> a = selectedWorkout.route.First();
            latitude = a.item1.Latitude;
            longitude = a.item1.Longitude;

           // selectedWorkout.routeCoordinates.First() 

        }

    }
}