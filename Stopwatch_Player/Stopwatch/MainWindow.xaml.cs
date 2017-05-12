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
        private System.Windows.Forms.NotifyIcon myNotifyIcon; 
        private DispatcherTimer timer;
        private TimeSpan timespanStart;
        private TimeSpan timespanEnd;
        public MainWindow()
        {
            InitializeComponent();

            myNotifyIcon = new System.Windows.Forms.NotifyIcon();
            myNotifyIcon.Icon = new System.Drawing.Icon(@"D:\stopwatch.ico");
            myNotifyIcon.MouseDoubleClick += myNotifyIcon_MouseDoubleClick;
            this.StateChanged += MainWindow_StateChanged;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;
            timespanStart = new TimeSpan();
            timespanEnd = new TimeSpan();
        }

        void myNotifyIcon_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                myNotifyIcon.BalloonTipTitle = "Minimize Sucessful";
                myNotifyIcon.BalloonTipText = "Minimized the app ";
                myNotifyIcon.ShowBalloonTip(400);
                myNotifyIcon.Visible = true;
            }
            else if (this.WindowState == WindowState.Normal)
            {
                myNotifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }

        void myNotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        //Обработчик таймера
        void timer_Tick(object sender, EventArgs e)
        {
            timespanEnd = DateTime.Now.TimeOfDay - timespanStart;
            tbTimer.Text = String.Format("{0:00}:{1:00}:{2:00}:{3:000}", timespanEnd.Hours, timespanEnd.Minutes, timespanEnd.Seconds, timespanEnd.Milliseconds);
        }

        //Нажатие на кнопку "ЗАПУСК"
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            lbMarks.Items.Clear();
            btnStopReset.Content = "СТОП";
            timer.Start();
            timespanStart = DateTime.Now.TimeOfDay;
            btnLap.IsEnabled = true;
        }

        //Нажатие на кнопку "СТОП (СБРОС)"
        private void btnStopReset_Click(object sender, RoutedEventArgs e)
        {
            btnLap.IsEnabled = false;
            if (timer.IsEnabled)
            {
                btnStopReset.Content = "СБРОС";
                timer.Stop();
            }
            else
            {
                btnStopReset.Content = "СТОП";
                tbTimer.Text = "00:00:00:000";
                lbMarks.Items.Clear();
            }
        }

        //Нажатие на кнопку "КРУГ"
        private void btnLap_Click(object sender, RoutedEventArgs e)
        {
            string newStringForListBox = String.Format("{0}-й круг! Время: {1:00}:{2:00}:{3:00}:{4:000}", lbMarks.Items.Count + 1, timespanEnd.Hours, timespanEnd.Minutes, timespanEnd.Seconds, timespanEnd.Milliseconds);
            lbMarks.Items.Add(newStringForListBox);
        }
    }
}
