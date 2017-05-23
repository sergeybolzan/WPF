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
        private List<string> playList = new List<string>();
        private DispatcherTimer timer;
        private bool fullScreen = false;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += timer_Tick;
            this.KeyDown += MainWindow_KeyDown;

        }

        void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (fullScreen == true) ChangeSizeScreen();
            }
            if (e.Key == Key.Enter)
            {
                ChangeSizeScreen();
            }
            if (e.Key == Key.Space) PlayPause();
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
            openFileDialog.Filter = "Video files(*.mp4;*.mkv;*.wmv;*.avi)|*.mp4;*.mkv;*.wmv;*.avi|All files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                //mediaElement.Source = new Uri(openFileDialog.FileName);
                playList.Clear();
                playList.AddRange(openFileDialog.FileNames);
                lbPlayList.ItemsSource = null;
                lbPlayList.ItemsSource = playList;
                lbPlayList.SelectedIndex = 0;
                mediaElement.Play();
            }
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                slPosition.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                btnPlayPause.IsEnabled = true;
                btnPreviousFile.IsEnabled = true;
                btnMoveBackward.IsEnabled = true;
                btnMoveForward.IsEnabled = true;
                btnNextFile.IsEnabled = true;
                timer.Start();
            }
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
            PlayPause();
        }

        private void PlayPause()
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

        private void CommandMinimizeBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void MenuItemFullScreen_Click(object sender, RoutedEventArgs e)
        {
            ChangeSizeScreen();
        }

        private void ChangeSizeScreen()
        {
            if (!fullScreen)
            {
                this.Content = mediaElement;
                LayoutRoot.Children.Remove(mediaElement);
                this.Background = new SolidColorBrush(Colors.Black);
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                TimeSpan position = mediaElement.Position; this.Content = LayoutRoot;
                LayoutRoot.Children.Add(mediaElement);
                this.Background = new SolidColorBrush(Colors.White);
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                mediaElement.Position = position;
            }
            fullScreen = !fullScreen;
        }

        private void btnNextFile_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayList.Items.Count == lbPlayList.SelectedIndex + 1) lbPlayList.SelectedIndex = 0;
            else lbPlayList.SelectedIndex++;
        }

        private void btnPreviousFile_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayList.SelectedIndex == 0) lbPlayList.SelectedIndex = lbPlayList.Items.Count - 1;
            else lbPlayList.SelectedIndex--;
        }
    }
}
