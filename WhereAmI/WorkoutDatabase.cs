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
//using System.Data.Linq.Mapping;
using System.Collections.Generic;

using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Device.Location;
using Microsoft.Phone.Controls.Maps;

namespace WhereAmI
{
    [Table]
    // backup: public class WorkoutDatabase : INotifyPropertyChanged, INotifyPropertyChanging
    public class WorkoutDatabase : INotifyPropertyChanged, INotifyPropertyChanging
    {
      public static ObservableCollection<Workout> workoutDB = new ObservableCollection<Workout>();


        public WorkoutDatabase()
        {

            Workout workout1 = new Workout();
            Workout workout2 = new Workout();
            Workout workout3 = new Workout();
            Workout workout4 = new Workout();
            Workout workout5 = new Workout();
            Workout workout6 = new Workout();

            workout1.workoutName = "Example Empty Workout1";
            workout2.workoutName = "Example Empty Workout2";
            workout3.workoutName = "Example Empty Workout3";
            workout4.workoutName = "Example Empty Workout4";
            workout5.workoutName = "Example Empty Workout5";
            workout6.workoutName = "Example Empty Workout6";

            //DOESN'T WORK, DON'T KNOW THE FUCK YET -_-!
            //workout1.routeLine.Locations = new LocationCollection();
            workout1.routeLine.Locations.Add(new GeoCoordinate(30.00, -100.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(31.00, -100.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(32.00, -100.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(33.00, -100.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -100.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -101.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -102.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -103.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -104.00));
            workout1.routeLine.Locations.Add(new GeoCoordinate(34.00, -105.00));

            workoutDB.Add(workout1);
            workoutDB.Add(workout2);
            workoutDB.Add(workout3);
            workoutDB.Add(workout4);
            workoutDB.Add(workout5);
            workoutDB.Add(workout6);

            System.Diagnostics.Debug.WriteLine("Sample workouts: ADDED");

        }



        public void addWorkoutToDatabase(Workout workout)
        {
            workoutDB.Add(workout);
        }

        public Workout findWorkoutByName(string stringItem)
        { //implement this, the content of this method is simply to avoid errors
            Workout workout = new Workout();

            return workout;
        }

        public ObservableCollection<Workout> getDatabase()
        {
            return workoutDB;
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this,
                              new PropertyChangedEventArgs(propertyName));
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

        public IEnumerator<Workout> GetEnumerator() //GOD!!!!! -_-'!!!
        {
            return workoutDB.GetEnumerator();
            //WHY can this.GetEnumerator(); be used?? and WHY specifically would it automatically take the workoutDB object? and NOT something else???

        }
    }
}
