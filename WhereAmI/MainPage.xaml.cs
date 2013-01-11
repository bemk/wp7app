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
using System.IO;
using System.Runtime.Serialization;
using System.IO.IsolatedStorage;
using System.Collections;

namespace WhereAmI
{
    public partial class MainPage : PhoneApplicationPage
    {
        public IGeoPositionWatcher<GeoCoordinate> geowatcher;                                               
        public bool AerialMode = false;
        public List<Tuple<GeoCoordinate, DateTime>> positions = new List<Tuple<GeoCoordinate, DateTime>>(); // store all GPS coordinates along with timestamps on each GPS registered coordinate
        public List<GeoCoordinate> routeCoordinates = new List<GeoCoordinate>();
        public MapPolyline joggingPolyLine { get; set; }     
        public double hourS, minS, secS, milliS = 0;
        public bool geoWatcherCheck = false;
        public PreviousWorkoutListPage previousWorkoutListPage = new PreviousWorkoutListPage();
        public static WorkoutDatabase mainDatabase;                             // THE one and only Database to be used
        protected bool started = false;
        protected bool lap = false;
        protected DispatcherTimer timer = null;
        protected DispatcherTimer speedLogTimer = null;
        protected DateTime startTime; 
        protected TimeSpan elapsed;
        protected TimeSpan elapsedTime;
        protected TimeSpan lapTime;
        public static double totalDistanceRan = 0;
        public string workoutDuration;
        public static DataSaver<WorkoutDatabase> dataSave = new DataSaver<WorkoutDatabase>();
        Workout newWorkout = new Workout();

        GeoCoordinate lastGPSlocation = new GeoCoordinate();
        public static WorkoutDatabase getDatabase()
        {
            return mainDatabase;
        }

        public MainPage()
        {
            InitializeComponent();
            mainDatabase = new WorkoutDatabase(); 
            lastGPSlocation = null;
            joggingPolyLine = new MapPolyline();                         
            joggingPolyLine.Stroke = new SolidColorBrush(Colors.Blue);
            joggingPolyLine.StrokeThickness = 5;
            joggingPolyLine.Opacity = 0.7;
            joggingPolyLine.Locations = new LocationCollection();
            map1.Children.Add(joggingPolyLine);
            string deviceName = Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceName").ToString();
            
            if (deviceName == "XDeviceEmulator")                                                                    // Checks for device name in order to determine whether to simulate a jogger or not
            {
                //Note: The following coordinates is just a Test coordinate for when running the GeoCoordinate simulator
               geowatcher = new GeoCoordinateSimulator(51.5941116666667, 4.77941666666667);                        
             
            }
            else
            {
                geowatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            }
            geowatcher.PositionChanged += watcher_PositionChanged;
            //The next line is responsible for loading the database from Isolated Storage. The loading and saving implementation works HOWEVER, It is NOT loaded properly and the previous workoutlist does not display previously saved workouts.
           // mainDatabase = dataSave.loadDatabaseFromIsolatedStorage("WorkoutDatabase");                                                 
 
           // checkIfFileExists();
        }

        // This method is called everytime the GPS position is changed
        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            myPositionText.Text = "Latitude: " + e.Position.Location.Latitude + "\nLongitude: " + e.Position.Location.Longitude;
            GeoCoordinate g = new GeoCoordinate(e.Position.Location.Latitude, e.Position.Location.Longitude);
            routeCoordinates.Add(g);
            DateTime t = new DateTime(DateTime.Now.Ticks);
            Tuple<GeoCoordinate, DateTime> tuple = new Tuple<GeoCoordinate, DateTime>(g, t);
            positions.Add(tuple);
            joggingPolyLine.Locations.Add(g);   
            map1.Center = g;
            map1.ZoomLevel = 16;

            if (positions.Count >= 2)
            {
                totalDistanceRan += g.GetDistanceTo(positions[positions.Count - 2].item1);
            }
            newWorkout.addKMpHtoKMpHcollection(calculateKMperHour(lastGPSlocation, g));
            

            System.Diagnostics.Debug.WriteLine(totalDistanceRan.ToString());
        }

        private void GeoPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
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

        private void locateMeButton_Click(object sender, RoutedEventArgs e)                         // When called, the map will indicate where your current position is
        {
            if (geoWatcherCheck == true)
            {
                double latitude = geowatcher.Position.Location.Latitude;
                double longitude = geowatcher.Position.Location.Longitude;
                map1.Center = new GeoCoordinate(latitude, longitude);
                map1.ZoomLevel = 16;
            }
            else { /*Do nothing please */}
        }

        private void endWorkoutButton_Click(object sender, RoutedEventArgs e)
        {
             
            MessageBoxResult m = MessageBox.Show("Are you sure you want to end your workout?","End Workout" , MessageBoxButton.OKCancel);
            if (m == MessageBoxResult.OK)
            {
              if (geoWatcherCheck == true)
                {
                    geowatcher.Stop();
                    geoWatcherCheck = false;
                    WorkoutSavePage.setWorkoutRoute(joggingPolyLine);
                    StopTimer();
                    WorkoutSavePage.setValues(startTime.ToString(), elapsedTime.ToString(), positions);
                    WorkoutSavePage.setTimesValues(startTime, elapsedTime);
                    totalDistanceRan = calculateDistance(positions);
                    WorkoutSavePage.workout = newWorkout;
                    NavigationService.Navigate(new Uri("/WorkoutSavePage.xaml", UriKind.RelativeOrAbsolute));
                }
              else {/* Don't do anything please */};
            }

            else { /*Do nothing please */}
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            button2.Visibility = Visibility.Visible;
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
            mainDatabase = DataSaver<WorkoutDatabase>.loadDatabaseFromIsolatedStorage("WorkoutDatabase");
            //if (mainDatabase != null)
            //{
            //    mainDatabase.returnAmountOfWorkoutsInDatabase();
            //}
            NavigationService.Navigate(new Uri("/PreviousWorkoutListPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += tick;                                         // Note: This is an actual self-implemented method
            timer.Interval = new TimeSpan(0,0,0,0,10);                  // 10 ms delay timer update
            timer.Start();
            startTime = DateTime.Now - elapsedTime;
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

            this.timerBlock.Text = string.Format("{0:00}:{1:00}:{2:00}.{3:0}",
                elapsed.Hours,
                elapsed.Minutes,
                elapsed.Seconds,
                elapsed.Milliseconds/100);
        }

        private void StopTimer()
        {
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

        public double calculateDistance(List<Tuple<GeoCoordinate, DateTime>> l)
        {
            Tuple<GeoCoordinate, DateTime>prev = null;
            double result = 0;
            foreach (Tuple<GeoCoordinate, DateTime> t in l) // <= LINQ query!!!
            {
                if (prev != null)
                {
                    result += prev.item1.GetDistanceTo(t.item1);
                }
                prev = t;
            }
            return result/1000;                                                     // return distance in Kilometers
        }

        public double calculateKMperHour(GeoCoordinate initialLocation, GeoCoordinate currentGPSlocation)
        {
            TimeSpan timeStamp = TimeSpan.Parse("00:00:01.00");
            double kmPerHour = 0;
            if (initialLocation != null)
            {
                double distance = initialLocation.GetDistanceTo(currentGPSlocation) / 1000;
                kmPerHour = distance / timeStamp.TotalHours;
            }

            if (initialLocation == null)
            {
                this.lastGPSlocation = currentGPSlocation;
                double distance = lastGPSlocation.GetDistanceTo(currentGPSlocation) / 1000;
                kmPerHour = distance / timeStamp.TotalHours;             
            }
            this.lastGPSlocation = currentGPSlocation;
            return kmPerHour;
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (map1.ZoomLevel == 16)
            {
                map1.ZoomLevel = 16;
            }
            else
            {
                map1.ZoomLevel += 1;
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (map1.ZoomLevel == 0)
            {
                map1.ZoomLevel = 0;
            }
            else
            {
                map1.ZoomLevel -= 1;
            }
        }
        
    }

}
