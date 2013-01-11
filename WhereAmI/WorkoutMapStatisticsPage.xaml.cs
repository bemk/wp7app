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
        MapLayer layer = new MapLayer();
        public WorkoutMapStatisticsPage()
        {
            InitializeComponent();
            routeLine = new MapPolyline();
            routeLine.Locations = selectedWorkout.routeLine.Locations; // Where the magic happens: Note: ALL routes MUST have a polyline, otherwise the application WILL ofcourse, CRASH!
            routeLine.Stroke = new SolidColorBrush(Colors.Blue);
            routeLine.StrokeThickness = 5;
            Pushpin pStart = new Pushpin();
            pStart.Content = "Start";
            pStart.Background = new SolidColorBrush(Colors.Green);
            Pushpin pFinish = new Pushpin();
            pFinish.Content = "Finish";
            pFinish.Background = new SolidColorBrush(Colors.Red);
            layer.AddChild(pStart, new GeoCoordinate(routeLine.Locations.First().Latitude, routeLine.Locations.First().Longitude));
            layer.AddChild(pFinish, new GeoCoordinate(routeLine.Locations.Last().Latitude, routeLine.Locations.Last().Longitude));
            map2.Children.Add(routeLine);
            map2.Children.Add(layer);
            textBlock5.Text = selectedWorkout.workoutName;
            textBlock6.Text = selectedWorkout.startTime;
            textBlock7.Text = String.Format("{0:F2}", selectedWorkout.distanceRan);
            textBlock8.Text = string.Format("{0:00}:{1:00}:{2:00}",
               selectedWorkout.elapsedTimeTS.Hours,
                selectedWorkout.elapsedTimeTS.Minutes,
                selectedWorkout.elapsedTimeTS.Seconds); ;
            
            double latitude;
            double longitude;
            getWorkoutRouteLocation(out latitude, out longitude);

            map2.Center = new GeoCoordinate(latitude, longitude);
            map2.ZoomLevel = 16;     
        }

        public static void setWorkout(Workout selectedWorkoutItem)
        {
            selectedWorkout = selectedWorkoutItem;
        }

        public static void getWorkoutRouteLocation(out double latitude, out double longitude)
        {
            Tuple<GeoCoordinate, DateTime> a = selectedWorkout.route.First();
            latitude = a.item1.Latitude;
            longitude = a.item1.Longitude;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (map2.ZoomLevel == 16)
            {
                map2.ZoomLevel = 16;
            }
            else
            {
                map2.ZoomLevel += 1;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (map2.ZoomLevel == 0)
            {
                map2.ZoomLevel = 0;
            }
            else
            {
                map2.ZoomLevel -= 1;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
           // WorkoutGraphPage.setWorkout(selectedWorkout);
            WorkoutGraphPage.selectedWorkout = selectedWorkout;
            NavigationService.Navigate(new Uri("/WorkoutGraphPage.xaml",UriKind.Relative));
        }

    }
}