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

namespace WhereAmI
{
    public partial class PreviousWorkoutListPage : PhoneApplicationPage
    {

        
        public PreviousWorkoutListPage()
        {
            InitializeComponent();
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Workout selectedWorkout = listBox1.SelectedItem as Workout;
            MessageBox.Show(selectedWorkout.workoutName + " is selected");
            WorkoutMapStatisticsPage.setWorkout(selectedWorkout);
            NavigationService.Navigate(new Uri("/WorkoutMapStatisticsPage.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            listBox1.ItemsSource = MainPage.dataSave.loadDatabaseFromIsolatedStorage("WorkoutDatabase").getDatabase();
            System.Diagnostics.Debug.WriteLine("db & listbox updated");
        }
    }
}