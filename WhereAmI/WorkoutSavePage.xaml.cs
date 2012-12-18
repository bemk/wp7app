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
        public static MainPage mainPage = new MainPage();
        private static MapPolyline mapPL = new MapPolyline();
        private static List<GeoCoordinate> routeCoordinates = new List<GeoCoordinate>();
        private static string elapsedTime;
        private static string workoutDuration;
        private static List<Tuple<GeoCoordinate, DateTime>> route;


        public WorkoutSavePage()
        {
            InitializeComponent();
            textBlock5.Text = elapsedTime;
            textBlock6.Text = MainPage.totalDistanceRan.ToString();
            textBlock7.Text  = workoutDuration;
        }

        public static void setValues(string currentElapsedTime, string currentWorkoutDuration, List<Tuple<GeoCoordinate, DateTime>> r)
        {
            elapsedTime = currentElapsedTime;
            workoutDuration = currentWorkoutDuration;
            route = r;
        }

        private void workoutNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           // workoutNameTextBox.Text = "Route1";
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
           Workout workout = new Workout();

           saveRoute(route, workoutNameTextBox.Text);

           workout.workoutName = workoutNameTextBox.Text;
           workout.startTime = DateTime.Now.ToString();
           workout.workoutDuration = workoutDuration;
          // workout.distanceRunned = Convert.ToDouble(kilometersTextBox.Text);
           //workout     workoutTimeTextBox
           workout.routeCoordinates = routeCoordinates;
           workout.distanceRan = MainPage.totalDistanceRan;
           workout.routeLine = mapPL;
           MainPage.mainDatabase.addWorkoutToDatabase(workout);
           NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));

        }

        public static void setWorkoutRoute(MapPolyline mpl)
        {
            mapPL = mpl;
        }

        public void saveRoute(List<Tuple<GeoCoordinate, DateTime>> route, string name)
        {
            XElement r = new XElement("Route",
                    new XAttribute("name", name),
                    from l in route.ToArray()
                    orderby l.item2.Ticks
                    select new XElement("Waypoint",
                        new XAttribute("lat", l.item1.Latitude),
                        new XAttribute("long", l.item1.Longitude),
                        new XAttribute("stamp", l.item2.Ticks)));

            System.Diagnostics.Debug.WriteLine(r);

            readRoute(r, name);

        }

        public void readRoute(XElement routes, string name)
        {
            var l = from r in routes.Elements("Route")
                     where r.Attribute("name").Value == name
                     select new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate((double)r.Element("Waypoint").Attribute("long"), (double)r.Element("Waypoint").Attribute("lat")),new DateTime((long)r.Element("Waypoint").Attribute("stamp")));
            foreach (var i in l)
                System.Diagnostics.Debug.WriteLine(i);
        }
    }
}
