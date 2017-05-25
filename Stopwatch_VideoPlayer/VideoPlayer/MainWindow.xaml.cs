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
            lbPlayList.ItemsSource = playList;
        }


        #region Menu events handlers
        //Файл - Открыть
        private void CommandOpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Video files(*.mp4;*.mkv;*.wmv;*.avi)|*.mp4;*.mkv;*.wmv;*.avi|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                playList.Clear();
                playList.AddRange(openFileDialog.FileNames);
                lbPlayList.Items.Refresh();
                lbPlayList.SelectedIndex = 0;
                mediaElement.Play();
            }
        }

        //Файл - Добавить в плейлист
        private void MenuItemAddToPlaylist_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.Filter = "Video files(*.mp4;*.mkv;*.wmv;*.avi)|*.mp4;*.mkv;*.wmv;*.avi|All files (*.*)|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                if (playList.Count == 0)
                {
                    EnableButtons();
                    btnPlayPause.Content = "Воспроизведение";
                    lbPlayList.SelectedIndex = 0;
                }
                playList.AddRange(openFileDialog.FileNames);
                lbPlayList.Items.Refresh();
            }
        }

        //Свернуть окно
        private void CommandMinimizeBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        //Во весь экран
        private void MenuItemFullScreen_Click(object sender, RoutedEventArgs e)
        {
            ChangeSizeScreen();
        }
        #endregion


        #region MediaElement events handlers
        //При открытии контента указываем таймлайну максимальное значение и активируем кнопки
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mediaElement.NaturalDuration.HasTimeSpan)
            {
                slPosition.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
                EnableButtons();
            }
        }

        //По завершению видео включаем следующее в плейлисте
        private void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (lbPlayList.Items.Count == lbPlayList.SelectedIndex + 1) lbPlayList.SelectedIndex = 0;
            else lbPlayList.SelectedIndex++;
        }
        #endregion


        #region Buttons clicks events handlers
        //Кнопка "-5". Перематываем назад на 5 сек
        private void btnMoveBackward_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position - TimeSpan.FromSeconds(5);
        }

        //Кнопка "+5". Перематываем вперед на 5 сек
        private void btnMoveForward_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Position = mediaElement.Position + TimeSpan.FromSeconds(5);
        }

        //Кнопка "Пауза/Воспроизведение"
        private void btnPlayStop_Click(object sender, RoutedEventArgs e)
        {
            PlayPause();
        }

        //Кнопка "Следующий". Если текущее видео - последнее, то включаем первое видео из плейлиста, либо включаем следующее
        private void btnNextFile_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayList.Items.Count == lbPlayList.SelectedIndex + 1) lbPlayList.SelectedIndex = 0;
            else lbPlayList.SelectedIndex++;
        }

        //Кнопка "Предыдущий". Если текущее видео - первое, то включаем последнее из плейлиста, либо включаем предыдущее 
        private void btnPreviousFile_Click(object sender, RoutedEventArgs e)
        {
            if (lbPlayList.SelectedIndex == 0) lbPlayList.SelectedIndex = lbPlayList.Items.Count - 1;
            else lbPlayList.SelectedIndex--;
        }
        #endregion


        #region Methods
        /// <summary>
        /// Воспроизводим или останавливаем видео
        /// </summary>
        private void PlayPause()
        {
            if (btnPlayPause.Content.ToString() == "Пауза")
            {
                mediaElement.Pause();
                btnPlayPause.Content = "Воспроизведение";
            }
            else
            {
                mediaElement.Play();
                btnPlayPause.Content = "Пауза";
            }
        }

        /// <summary>
        /// Включаем либо выключаем полноэкранный режим
        /// </summary>
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

        /// <summary>
        /// Активируем кнопки и запускаем таймер
        /// </summary>
        private void EnableButtons()
        {
            btnPlayPause.IsEnabled = true;
            btnPreviousFile.IsEnabled = true;
            btnMoveBackward.IsEnabled = true;
            btnMoveForward.IsEnabled = true;
            btnNextFile.IsEnabled = true;
            timer.Start();
        }
        #endregion


        //Обработчик таймера. Смещается таймлайн и меняется лейбл с текущим временем воспроизведения
        private void timer_Tick(object sender, EventArgs e)
        {
            slPosition.Value = mediaElement.Position.TotalMilliseconds;
            lbTime.Content = mediaElement.Position.ToString(@"hh\:mm\:ss");
        }

        //Перемещаем воспроизведение видео на позицию, соответствующую положению таймлайна
        private void timeLineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromMilliseconds(slPosition.Value);
        }

        //Хоткеи. "Enter" - вкл/выкл полноэкранного режима. "Пробел" - Воспроизведение/Пауза
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
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
    }
}
