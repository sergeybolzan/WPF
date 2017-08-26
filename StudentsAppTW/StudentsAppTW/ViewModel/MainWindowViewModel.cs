using StudentsAppTW.Infrastructure;
using StudentsAppTW.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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


        #region Fields and properties
        private ObservableCollection<Student> students;
        public ObservableCollection<Student> Students   
        {
            get
            {
                if (students == null)
                    students = XmlHelper.XmlDeserialize<Students>("Students.xml").Student;
                return students;
            }
        }

        private Student selectedStudent;
        public Student SelectedStudent
        {
            get
            {
                return selectedStudent ?? (selectedStudent = new Student());
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

        private Student currentStudent;
        public Student CurrentStudent
        {
            get
            {
                return currentStudent ?? (currentStudent = new Student());
            }
            set
            {
                currentStudent = value;
                OnPropertyChanged("CurrentStudent");
            }
        }

        private bool isPopupOnEmptyShow;
        public bool IsPopupOnEmptyShow
        {
            get { return isPopupOnEmptyShow; }
            set 
            { 
                isPopupOnEmptyShow = value;
                OnPropertyChanged("IsPopupOnEmptyShow");
            }
        }

        private bool isPopupOnOutOfRangeShow;
        public bool IsPopupOnOutOfRangeShow
        {
            get { return isPopupOnOutOfRangeShow; }
            set
            {
                isPopupOnOutOfRangeShow = value;
                OnPropertyChanged("IsPopupOnOutOfRangeShow");
            }
        }
        #endregion // Fields and properties


        #region Commands
        private RelayCommand addStudent;
        public RelayCommand AddStudent
        {
            get
            {
                return addStudent ?? (addStudent = new RelayCommand(obj =>
                    {
                        if (string.IsNullOrEmpty(CurrentStudent.FirstName) || string.IsNullOrEmpty(CurrentStudent.Last))
                        {
                            IsPopupOnEmptyShow = true;
                            return;
                        }
                        if (CurrentStudent.Age < 16 || CurrentStudent.Age > 100)
                        {
                            IsPopupOnOutOfRangeShow = true;
                            return;
                        }
                        if (Students.Count == 0) CurrentStudent.Id = 0;
                        else CurrentStudent.Id = Students.Max(x => x.Id) + 1;
                        Students.Add(CurrentStudent);
                        XmlHelper.AddNewStudentToXml(CurrentStudent);
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
                        if (string.IsNullOrEmpty(CurrentStudent.FirstName) || string.IsNullOrEmpty(CurrentStudent.Last))
                        {
                            IsPopupOnEmptyShow = true;
                            return;
                        }
                        if (CurrentStudent.Age < 16 || CurrentStudent.Age > 100)
                        {
                            IsPopupOnOutOfRangeShow = true;
                            return;
                        }
                        SelectedStudent.FirstName = CurrentStudent.FirstName;
                        SelectedStudent.Last = CurrentStudent.Last;
                        SelectedStudent.Age = CurrentStudent.Age;
                        SelectedStudent.Gender = CurrentStudent.Gender;
                        XmlHelper.EditStudentInXml(SelectedStudent);
                    }, (obj) => Students.Count > 0));
            }
        }

        private RelayCommand removeStudent;
        public RelayCommand RemoveStudent
        {
            get
            {
                return removeStudent ?? (removeStudent = new RelayCommand(obj =>
                    {
                        IList removableStudents = obj as IList;
                        if (removableStudents.Count > 0)
                        {
                            var result = MessageBox.Show("Вы уверены что хотите удалить записи? Действие необратимо.", "Удаление", MessageBoxButton.OKCancel);
                            if (result == MessageBoxResult.OK)
                            {
                                XmlHelper.RemoveStudentsFromXml(removableStudents);
                                for (int i = removableStudents.Count - 1; i >= 0; i--)
                                {
                                    var student = removableStudents[i] as Student;
                                    Students.Remove(student);
                                }
                            }
                        }
                    }, (obj) => Students.Count > 0));
            }
        }
        #endregion // Commands
    }
}
