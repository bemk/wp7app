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
using System.Device.Location;
using SharpGIS.WinPhone.Gps;
using Microsoft.Phone.Controls.Maps.Platform;
using System.Windows.Threading;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace WhereAmI
{
    public partial class MainPage : PhoneApplicationPage
    {
        public IGeoPositionWatcher<GeoCoordinate> geowatcher;                  // necessary variable for jogging simulation
        public bool AerialMode = false;
        public List<GeoCoordinate> geoPositions = new List<GeoCoordinate>();   // store all GPS coordinates
        public MapPolyline joggingPolyLine;  
        public double hourS, minS, secS, milliS = 0;

        public bool geoWatcherCheck = false;
        public MainPage()
        {
            InitializeComponent();
            map1.ZoomBarVisibility = Visibility.Visible;                       // Zoom Buttons Visibility 
            joggingPolyLine = new MapPolyline();                               //jogging path visualization
            joggingPolyLine.Stroke = new SolidColorBrush(Colors.Blue);
            joggingPolyLine.StrokeThickness = 5;
            joggingPolyLine.Opacity = 0.7;
            joggingPolyLine.Locations = new LocationCollection();
            map1.Children.Add(joggingPolyLine); 

            string deviceName = Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceName").ToString();

            if (deviceName == "XDeviceEmulator")    // Checks for device name in order to determine whether to simulate a jogger or not
                    { 
                        geowatcher = new GeoCoordinateSimulator(34.0568, -117.195); // Test coordinates when running the simulator
                    }
                    else
                    {
                        geowatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                    }
                    geowatcher.PositionChanged += watcher_PositionChanged; 
                                 
        }


        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            myPositionText.Text = "Latitude: " + e.Position.Location.Latitude + "\n Longitude: " + e.Position.Location.Longitude;
            geoPositions.Add(new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude));
            joggingPolyLine.Locations.Add(new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude));
        }

        void GeoPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {

        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CenterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SwitchViewButton_Click(object sender, RoutedEventArgs e)
        {
            AerialMode = !AerialMode;
            if (AerialMode)
            {
                map1.Mode = new AerialMode();
            }
            else
            {
                map1.Mode = new RoadMode();
            }
        }

        private void locateMeButton_Click(object sender, RoutedEventArgs e) // When called, the map will indicate where the your current position is
        {
            if (geoWatcherCheck == true)
            {
                double latitude = geowatcher.Position.Location.Latitude;
                double longitude = geowatcher.Position.Location.Longitude;
                map1.Center = new GeoCoordinate(geowatcher.Position.Location.Latitude, geowatcher.Position.Location.Longitude);
                map1.ZoomLevel = 16;
            }
            else {/*Do nothing please */}
        }

        private void endWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoWatcherCheck == true)
            {
                geowatcher.Stop();
                geoWatcherCheck = false;
            }
            else {/*Do nothing please */}
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoWatcherCheck == false)
            {
                geowatcher.Start();
                geoWatcherCheck = true;
            }
            else { /*Do nothing please */} 
        }
    }

}