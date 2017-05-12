using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Stopwatch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        TimeSpan timespanStart;
        TimeSpan timespanEnd;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timespanStart = new TimeSpan();
            timespanEnd = new TimeSpan();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //timespan += timer.Interval;
            //tbTimer.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:000}", timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
            //tbTimer.Text = DateTime.Now.ToString("HH:mm:ss.fff");

            timespanEnd = DateTime.Now.TimeOfDay - timespanStart;
            tbTimer.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:000}", timespanEnd.Hours, timespanEnd.Minutes, timespanEnd.Seconds, timespanEnd.Milliseconds);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
            timespanStart = DateTime.Now.TimeOfDay;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lbMarks.Items.Add(timespanEnd);
        }
    }
}
