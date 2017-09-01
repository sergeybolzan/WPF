using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveProcesses
{
    class MainWindowVievModel: INotifyPropertyChanged
    {
        private Process[] processes;
        public Process[] Processes
        {
            get { return processes; }
            set 
            { 
                processes = value;
                OnPropertyChanged("Processes");
            }
        }
        

        private RelayCommand showProcesses;
        public RelayCommand ShowProcesses
        {
            get
            {
                return showProcesses ?? (showProcesses = new RelayCommand(obj =>
                    {
                        Processes = Process.GetProcesses().OrderBy(x => x.ProcessName).ToArray();
                    }));
            }
        }


        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion // INotifyPropertyChanged Members    
    }
}
