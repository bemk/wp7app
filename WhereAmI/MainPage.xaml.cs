//Last ONE!!

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
using System.Threading;

namespace WhereAmI
{
    public partial class MainPage : PhoneApplicationPage
    {
        public IGeoPositionWatcher<GeoCoordinate> geowatcher;                  // necessary variable for jogging simulation
        public bool AerialMode = false;
        //public List<GeoCoordinate> geoPositions = new List<GeoCoordinate>();   // store all GPS coordinates
        public List<Tuple<GeoCoordinate, DateTime>> positions = new List<Tuple<GeoCoordinate, DateTime>>();
        public MapPolyline joggingPolyLine { get; set; }     
        public double hourS, minS, secS, milliS = 0;
        public bool geoWatcherCheck = false;
        public PreviousWorkoutListPage previousWorkoutListPage = new PreviousWorkoutListPage();

        // DistanceCalculator calculate = new DistanceCalculator();
        //public WorkoutSavePage workoutSavePage = new WorkoutSavePage(); DO NOT DO THIS IF YOU WANT HAVE JUST ONE VARIABLE!
        // YOU HAVE TO MAKE EVERYTHING STATIC.
        // They one and true way to access this one and only variable is to type it in as a method/ property!

        public static WorkoutDatabase mainDatabase = new WorkoutDatabase();


        protected bool started = false;
        protected bool lap = false;
        protected DispatcherTimer timer = null;
        protected DateTime startTime; // use this for workout object DATE
        protected TimeSpan elapsed;
        protected TimeSpan elapsedTime;
        protected TimeSpan lapTime;

        public static double totalDistanceRan = 0;

        public string workoutDuration; // use this for workout object WORKOUTDURATION
 
        public static WorkoutDatabase getDatabase()
        {
            return mainDatabase;
        }
        public MainPage()
        {
            InitializeComponent();
            // map1.ZoomBarVisibility = Visibility.Visible;                       // Zoom Buttons Visibility 
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

            GeoCoordinate g = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            DateTime t = new DateTime(DateTime.Now.Ticks);
            Tuple<GeoCoordinate, DateTime> tuple = new Tuple<GeoCoordinate, DateTime>(g, t);
            positions.Add(tuple);

            joggingPolyLine.Locations.Add(g);
            
            map1.Center = g;
            map1.ZoomLevel = 16;
  
            if (positions.Count >= 2)
                totalDistanceRan += g.GetDistanceTo(positions[positions.Count - 2].item1);
            System.Diagnostics.Debug.WriteLine(totalDistanceRan.ToString());
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

        private void locateMeButton_Click(object sender, RoutedEventArgs e) // When called, the map will indicate where your current position is
        {
            if (geoWatcherCheck == true)
            {
                double latitude = geowatcher.Position.Location.Latitude;
                double longitude = geowatcher.Position.Location.Longitude;
                map1.Center = new GeoCoordinate(latitude, longitude);
                map1.ZoomLevel = 16;
                //Console.WriteLine("Time of Travel: {0:dd\\.hh\\:mm\\:ss} days", lapTime);
            }
            else { /*Do nothing please */}

        }

        private void endWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoWatcherCheck == true)
            {
                geowatcher.Stop();
                geoWatcherCheck = false;
                WorkoutSavePage.setWorkoutRoute(joggingPolyLine);
                StopTimer();
                WorkoutSavePage workoutSavePage = new WorkoutSavePage();

                workoutSavePage.setValues(startTime.ToString(), elapsedTime.ToString());

               // workoutSavePage.textBlock5.Text = startTime.ToString();
               // workoutSavePage.textBlock6.Text =  CALCULATE DISTANCE BETWEEN FIRST AND LAST POINT and RETURN in KILOMETERS
               // workoutSavePage.textBlock7.Text = elapsedTime.ToString();
                totalDistanceRan = dist(positions);
                NavigationService.Navigate(new Uri("/WorkoutSavePage.xaml", UriKind.RelativeOrAbsolute));
            }
            else { /*Do nothing please */}
        }



        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (geoWatcherCheck == false)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now);
                geowatcher.Start();
                geoWatcherCheck = true;
                StartTimer();
                
            }
            else { /*Do nothing please */}
        }


        private void displayPreviousWorkoutButton_Click(object sender, RoutedEventArgs e)
        {

            //foreach (Workout w in (IEnumerable<Workout>)mainDatabase)
            //{
            //    TextBlock workoutBlock = new TextBlock();
            //    workoutBlock.Text = w.workoutName;
            //    workoutPage.workoutStackPanel.Children.Add(workoutBlock);
            //    System.Diagnostics.Debug.WriteLine("Item :" + w.workoutName);
            //}
            //System.Diagnostics.Debug.WriteLine("Done going through list");

            foreach (Workout w in mainDatabase) // THIS IS MY NEWLY ENGINEERED foreach loop!!
            {

                TextBlock workoutBlock = new TextBlock();
                workoutBlock.Text = w.workoutName;
                //workoutPage.workoutStackPanel.Children.Add(workoutBlock);

                System.Diagnostics.Debug.WriteLine("Item :" + w.workoutName);
            }
            //previousWorkoutListPage.setDatabase(mainDatabase);
            System.Diagnostics.Debug.WriteLine("Button Pressed");
            NavigationService.Navigate(new Uri("/PreviousWorkoutListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += tick; // tick is the actual tick method!!!!!! NOTE THAT!!
            timer.Interval = new TimeSpan(0,0,0,0,10); // 1000 ms * 20 hz - 3 = 47 ms delay
            timer.Start();

            startTime = DateTime.Now - elapsedTime;

          //  lapButton.Enabled = true;
        }
        protected void tick(object sender, EventArgs e)
        {
            updateClock();
        }

        private void updateClock()
        {

            elapsedTime = DateTime.Now - startTime;

            if (lap)
            {
                elapsed = lapTime;
            }
            else
            {
                elapsed = elapsedTime;
            }
            //workoutDuration = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
            //    elapsed.Hours,
            //    elapsed.Minutes,
            //    elapsed.Seconds,
            //    elapsed.Milliseconds/100);

            this.timerBlock.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                elapsed.Hours,
                elapsed.Minutes,
                elapsed.Seconds,
                elapsed.Milliseconds/100);
        }

        private void StopTimer()
        {
            //lapButton.Enabled = false;
            workoutDuration = elapsedTime.ToString();
            System.Diagnostics.Debug.WriteLine(elapsedTime.ToString());
            if (timer != null)
            {
                timer.Stop();
                setTimerToNull();
            }
            
        }

        private void setTimerToNull()
        {
            timer = null;
        }

        public void ishviosjvivjsiojsvoid()
        {
           
        }

        public void huidigeTijdOpslaan()
        {
            //huidigeTijdStamp = DateTime.Now;
           // System.Diagnostics.Debug.WriteLine("Huidige Tijd Stamp object heeft nu als tijd: "+ huidigeTijdStamp.ToString());

        }

        #region garbage
        public double dist(List<Tuple<GeoCoordinate, DateTime>> l)
        {
            Tuple<GeoCoordinate, DateTime>prev = null;
            double result = 0;
            foreach (Tuple<GeoCoordinate, DateTime> t in l)
            {
                if (prev != null)
                {
                    result += prev.item1.GetDistanceTo(t.item1);
                }
                prev = t;
            }
            return result/1000;
        }
        #endregion

    }

}