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
using System.Collections.ObjectModel;

namespace WhereAmI
{
    public partial class WorkoutGraphPage : PhoneApplicationPage
    {
        PointCollection valuesCollection = new PointCollection();
        double second = 0.0;
        public static Workout selectedWorkout { get; set; }

        public WorkoutGraphPage()
        {
            InitializeComponent();

            foreach (double kmPerHour in selectedWorkout.kmPerHour)
            {//By the way: x Axis seconds and the y Axis indicates KM/H
                valuesCollection.Add(new Point(second++, kmPerHour));
            }  
            this.MyLineSeriesChart.DataContext = valuesCollection;
      
        }

        private void LineChart_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}