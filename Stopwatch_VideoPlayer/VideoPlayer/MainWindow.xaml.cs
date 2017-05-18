using Microsoft.Win32;
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

namespace VideoPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += timer_Tick;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            slPosition.Value = mediaElement.Position.TotalMilliseconds;
            lbTime.Content = mediaElement.Position.ToString(@"hh\:mm\:ss");
        }

        private void CommandOpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Video Files(*.mp4;*.mkv;*.wmv;*.avi)|*.mp4;*.mkv;*.wmv;*.avi|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                mediaElement.Source = new Uri(openFileDialog.FileName);
                mediaElement.Play();
            }
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            slPosition.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            btnPlayPause.IsEnabled = true;
            btnPreviousFile.IsEnabled = true;
            btnMoveBackward.IsEnabled = true;
            btnMoveForward.IsEnabled = true;
            btnNextFile.IsEnabled = true;
            timer.Start();

        }

        private void timeLineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromMilliseconds(slPosition.Value);

        }

        private void btnMoveBackward_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position - TimeSpan.FromSeconds(5);
        }

        private void btnMoveForward_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position + TimeSpan.FromSeconds(5);
        }

        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            //timer.Stop();
        }

        private void btnPlayStop_Click(object sender, RoutedEventArgs e)
        {
            if (btnPlayPause.Content.ToString() == "Pause")
            {
                mediaElement.Pause();
                btnPlayPause.Content = "Play";
            }
            else
            {
                mediaElement.Play();
                btnPlayPause.Content = "Pause";

            }
        }
    }
}
