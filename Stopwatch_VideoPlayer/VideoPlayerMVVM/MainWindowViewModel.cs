using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Win32;
using System;

namespace VideoPlayerMVVM
{ 
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<string> playList;
        public ObservableCollection<string> PlayList
        {
            get
            {
                if (playList == null) playList = new ObservableCollection<string>();
                return playList;
            }
        }



        private RelayCommand openFiles;
        public RelayCommand OpenFiles
        {
            get
            {
                return openFiles ?? (openFiles = new RelayCommand(obj =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                        openFileDialog.Multiselect = true;
                        openFileDialog.Filter = "Video files(*.mp4;*.mkv;*.wmv;*.avi)|*.mp4;*.mkv;*.wmv;*.avi|All files (*.*)|*.*";
                        if (openFileDialog.ShowDialog() == true)
                        {
                            foreach (var item in openFileDialog.FileNames)
                            {
                                PlayList.Add(item);
                            }
                        }
                    }));
            }
        }
    }
}
