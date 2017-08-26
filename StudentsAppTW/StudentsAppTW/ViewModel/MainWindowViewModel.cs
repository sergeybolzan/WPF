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

        ObservableCollection<StudentsStudent> students;
        public ObservableCollection<StudentsStudent> Students   
        {
            get
            {
                if (students == null)
                    students = XmlHelper.XmlDeserialize<Students>("Students.xml").Student;//ClientRepository.AllClients;
                return students;
            }
        }

        private StudentsStudent selectedStudent = new StudentsStudent();
        public StudentsStudent SelectedStudent
        {
            get
            {
                return selectedStudent ?? (selectedStudent = new StudentsStudent());
            }
            set
            {
                selectedStudent = value;
                OnPropertyChanged("SelectedStudent");
                if (selectedStudent != null)
                {
                    CurrentStudent.FirstName = selectedStudent.FirstName;
                    CurrentStudent.Last = selectedStudent.Last;
                    CurrentStudent.Age = selectedStudent.Age;
                    CurrentStudent.Gender = selectedStudent.Gender;
                }
            }
        }

        private StudentsStudent currentStudent = new StudentsStudent();
        public StudentsStudent CurrentStudent
        {
            get
            {
                return currentStudent ?? (currentStudent = new StudentsStudent());
            }
            set
            {
                currentStudent = value;
                OnPropertyChanged("CurrentStudent");
            }
        }

        private RelayCommand addStudent;
        public RelayCommand AddStudent
        {
            get 
            {
                return addStudent ?? (addStudent = new RelayCommand(obj =>
                    {
                        CurrentStudent.Id = Students.Count;
                        Students.Add(CurrentStudent);
                        CurrentStudent = null;
                    }));
            }
        }

        private RelayCommand editStudent;
        public RelayCommand EditStudent
        {
            get
            {
                return editStudent ?? (editStudent = new RelayCommand(obj =>
                {
                    SelectedStudent.FirstName = CurrentStudent.FirstName;
                    SelectedStudent.Last = CurrentStudent.Last;
                    SelectedStudent.Age = CurrentStudent.Age;
                    SelectedStudent.Gender = CurrentStudent.Gender;
                }));
            }
        }

        private RelayCommand deleteStudent;
        public RelayCommand DeleteStudent
        {
            get
            {
                return deleteStudent ?? (deleteStudent = new RelayCommand(obj =>
                {
                    if (SelectedStudent != null)
                    {
                        Students.Remove(SelectedStudent);
                    }
                }));
            }
        }

    }
}
