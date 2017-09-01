using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsAppTW.Model
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Students
    {

        private ObservableCollection<Student> studentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Student")]
        public ObservableCollection<Student> Student
        {
            get
            {
                return this.studentField;
            }
            set
            {
                this.studentField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Student : INotifyPropertyChanged
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

        private string firstNameField;

        private string lastField;

        private int ageField;

        private bool genderField;

        private int idField;

        /// <remarks/>
        public string FirstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
                OnPropertyChanged("FirstName");
            }
        }

        /// <remarks/>
        public string Last
        {
            get
            {
                return this.lastField;
            }
            set
            {
                this.lastField = value;
                OnPropertyChanged("Last");
            }
        }

        /// <remarks/>
        public int Age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
                OnPropertyChanged("Age");
            }
        }

        /// <remarks/>
        public bool Gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
                OnPropertyChanged("Gender");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                OnPropertyChanged("Id");
            }
        }
    }
}
