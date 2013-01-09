using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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
using System.Device.Location;

namespace WhereAmI
{
    public partial class WorkoutSavePage : PhoneApplicationPage
    {
        private static MapPolyline mapPL = new MapPolyline();
        private static List<GeoCoordinate> routeCoordinates = new List<GeoCoordinate>();
        private static string elapsedTime;
        private static string workoutDuration;
        private static List<Tuple<GeoCoordinate, DateTime>> route;
        public static Workout workout { get; set; }

        private static DateTime startTime;
        private static TimeSpan elapsedTimeTS ;

        public WorkoutSavePage() 
        {
            
            InitializeComponent();
            //elapsedTimeTS;
            textBlock5.Text = startTime.ToString();
            textBlock6.Text = String.Format("{0:F2}", MainPage.totalDistanceRan);
            textBlock7.Text = string.Format("{0:00}:{1:00}:{2:00}",
                elapsedTimeTS.Hours,
                elapsedTimeTS.Minutes,
                elapsedTimeTS.Seconds) ;
        }

        public static void setValues(string currentElapsedTime, string currentWorkoutDuration, List<Tuple<GeoCoordinate, DateTime>> r)
        {
            elapsedTime = currentElapsedTime;
            workoutDuration = currentWorkoutDuration;
            route = r;
        }

        private void workoutNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void saveWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
          

           saveRoute(route, workoutNameTextBox.Text);

           workout.workoutName = workoutNameTextBox.Text;
           workout.startTime = DateTime.Now.ToString();
           workout.workoutDuration = workoutDuration;
           workout.routeCoordinates = routeCoordinates;
           workout.distanceRan = MainPage.totalDistanceRan;
           workout.routeLine = mapPL;
           workout.elapsedTimeTS = elapsedTimeTS;
           workout.route = route;

            // I suspect that the possible culprit to our loading and saving of the database object lies here, in the next two lines(but I'm not sure):
           

           MainPage.mainDatabase.addWorkoutToDatabase(workout);
           MainPage.dataSave.saveDatabaseToIsolatedStorage(MainPage.mainDatabase, "WorkoutDatabase");

           NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
        }

        public static void setWorkoutRoute(MapPolyline mpl)
        {
            mapPL = mpl;
        }

        public void saveRoute(List<Tuple<GeoCoordinate, DateTime>> route, string name)
        {
            XElement r = new XElement("Routes",
                new XElement("Route",
                    new XAttribute("name", name),
                    from l in route.ToArray()
                    orderby l.item2.Ticks
                    select new XElement("Waypoint",
                        new XAttribute("lat", l.item1.Latitude),
                        new XAttribute("long", l.item1.Longitude),
                        new XAttribute("stamp", l.item2.Ticks))));

            System.Diagnostics.Debug.WriteLine(r);

            readRoute(r, name);

        }

        public void readRoute(XElement routes, string name)
        {
            var temp = routes.Elements("Route").Where(Route => Route.Attribute("name").Value.Equals(name)).Select(Route => new XElement(Route));
            var result = temp.Elements("Waypoint").Select(wp => new Tuple<GeoCoordinate, DateTime>
                (
                    new GeoCoordinate(
                        double.Parse(wp.Attribute("long").Value), 
                        double.Parse(wp.Attribute("lat").Value)),
                    new DateTime(long.Parse(wp.Attribute("stamp").Value))
                ));

            foreach (var i in result)
                System.Diagnostics.Debug.WriteLine(i.item1.ToString() + i.item2.ToString());
        }

        internal static void setTimesValues(DateTime currentStartTime, TimeSpan currentElapsedTime)
        {
            startTime = currentStartTime;
            elapsedTimeTS = currentElapsedTime;
        }
    }
}
