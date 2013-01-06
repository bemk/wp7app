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
        public static ObservableCollection<Workout> workoutDatabase = workoutDatabase = new ObservableCollection<Workout>();
        DataSaver<WorkoutDatabase> databaseLoader = new DataSaver<WorkoutDatabase>();
        public WorkoutDatabase()
        {
            
      

           // if(databaseLoader.getIsolatedStorageFile()!=null)
           // workoutDatabase = databaseLoader.loadDatabaseFromIsolatedStorage("WorkoutDatabase").getDatabaseCollection();
        }

        public int returnAmountOfWorkoutsInDatabase()
        {
            int amountOfWorkoutsInDatabase = 0;
         //   if (workoutDatabase != null)
         //   {
                foreach (Workout workout in workoutDatabase)
                {
                    amountOfWorkoutsInDatabase++;
                }
        //    }
            System.Diagnostics.Debug.WriteLine("The amount of workouts in Database is " + amountOfWorkoutsInDatabase);
            return amountOfWorkoutsInDatabase;
        }

        public void addWorkoutToDatabase(Workout workout)
        {
            workoutDatabase.Add(workout);
        }

        internal ObservableCollection<Workout> getDatabaseCollection()
        {
            return workoutDatabase;
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