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
        private static string elapsedTime;
        private static string workoutDuration;


        public WorkoutSavePage()
        {
            InitializeComponent();
            textBlock5.Text = elapsedTime;
            textBlock6.Text = MainPage.totalDistanceRan.ToString();
            textBlock7.Text  = workoutDuration;
        }

        public static void setValues(string currentElapsedTime, string currentWorkoutDuration)
        {
            elapsedTime = currentElapsedTime;
            workoutDuration = currentWorkoutDuration;
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

           saveRoute(mainPage.positions, workoutNameTextBox.Text);

           workout.workoutName = workoutNameTextBox.Text;
           workout.startTime = DateTime.Now.ToString();
           workout.workoutDuration = workoutDuration;
          // workout.distanceRunned = Convert.ToDouble(kilometersTextBox.Text);
           //workout     workoutTimeTextBox
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
            XElement r = new XElement("Routes",
                new XElement("Route",
                    new XAttribute("name", name),
                    from l in route.ToArray()
                    select new XElement("Waypoint",
                        new XAttribute("Lat", l.item1.Latitude),
                        new XAttribute("long", l.item1.Longitude),
                        new XAttribute("stamp", l.item2.Ticks))));

            System.Diagnostics.Debug.WriteLine(r);
        }
    }
}