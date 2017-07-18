using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskListApp2.Models
{
    public class Task : INotifyPropertyChanged
    {
        public int Id { get; set; }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value;
            RaisePropertyChanged("Name");
            }
        }
        
        public int Workload { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Person { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
