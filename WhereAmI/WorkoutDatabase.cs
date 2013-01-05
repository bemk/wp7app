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
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;

namespace WhereAmI
{
    [Table, DataContract]
    public class WorkoutDatabase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        [DataMember]
        public static ObservableCollection<Workout> workoutDatabase = new ObservableCollection<Workout>();
      
        public WorkoutDatabase()
        {

            //Workout workout1 = new Workout();
            //Workout workout2 = new Workout();
            //Workout workout3 = new Workout();
            //Workout workout4 = new Workout();
            //Workout workout5 = new Workout();
            //Workout workout6 = new Workout();
            //workout1.workoutName = "Workout1 my first workout";
            //workout2.workoutName = "Workout2 Weehee New Record! :D";
            //workout3.workoutName = "Workout3 stupid rainy day..";
            //workout4.workoutName = "Workout4 Reaching Goal Weight!";
            //workout5.workoutName = "10kM";
            //workout6.workoutName = "run with Creg";
            //workout1.route = new List<Tuple<GeoCoordinate, DateTime>>();
            //DOESN'T WORK, DON'T KNOW THE FUCK YET -_-!
            //workout1.routeLine.Locations = new LocationCollection();
            //workout1.route.Locations.Add(new List<Tuple<GeoCoordinate>>(30.00, -100.00));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.00, -100.00), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.01, -100.01), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.02, -100.02), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.03, -100.03), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.04, -100.08), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.05, -100.05), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.06, -100.02), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.07, -100.04), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.08, -100.02), new DateTime()));
            //workout1.route.Add(new Tuple<GeoCoordinate, DateTime>(new GeoCoordinate(30.09, -100.01), new DateTime()));
            //loadedDatabase.Add(workout1);
            //loadedDatabase.Add(workout2);
            //loadedDatabase.Add(workout3);
            //loadedDatabase.Add(workout4);
            //loadedDatabase.Add(workout5);
            //loadedDatabase.Add(workout6);
            //System.Diagnostics.Debug.WriteLine("Sample workouts: ADDED");
         //  saveDatabaseToIsolatedStorage();
            
         //   loadedDatabase = loadDatabaseFromIsolatedStorage();
        }

        public int returnAmountOfWorkoutsInDatabase()
        {
            int amountOfWorkoutsInDatabase = 0;
            foreach (Workout workout in workoutDatabase)
            {
                amountOfWorkoutsInDatabase++;
            }
            System.Diagnostics.Debug.WriteLine("The amount of workouts in Database is " + amountOfWorkoutsInDatabase);
            return amountOfWorkoutsInDatabase;
        }

        public void addWorkoutToDatabase(Workout workout)
        {
            workoutDatabase.Add(workout);
        }

        public Workout findWorkoutByName(string stringItem)
        { //implement this, the content of this method is simply to avoid errors
            Workout workout = new Workout();

            return workout;
        }

        public ObservableCollection<Workout> getDatabase()
        {
            return workoutDatabase;
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this,
                              new PropertyChangingEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public IEnumerator<Workout> GetEnumerator() // -_-'!!! One annoying method!
        {
            return workoutDatabase.GetEnumerator();
            //WHY can this.GetEnumerator(); be used?? and WHY specifically would it automatically take the workoutDB object? and NOT something else???
        }
    }
}