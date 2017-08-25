using StudentsAppTW.Infrastructure;
using StudentsAppTW.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsAppTW.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
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

        ObservableCollection<StudentsStudent> _clients;
        public ObservableCollection<StudentsStudent> Clients
        {
            get
            {
                if (_clients == null)
                    _clients = XmlHelper.XmlDeserialize<Students>("Students.xml").Student;//ClientRepository.AllClients;
                return _clients;
            }
        }
        
    }
}
