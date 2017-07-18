namespace TaskListApp2.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using System;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Windows;
    using TaskListApp2.Models;

    public class AddTaskDialogViewModel : ViewModelBase
    {
        public AddTaskDialogViewModel()
        {
            NewTask = new Task();
        }

        
        private ObservableCollection<Person> persons;
        public ObservableCollection<Person> Persons
        {
            get
            {
                if (persons == null) persons = PersonRepository.Persons;
                return persons;
            }
        }
        private ObservableCollection<Status> statuses;
        public ObservableCollection<Status> Statuses
        {
            get
            {
                if (statuses == null) statuses = StatusRepository.Statuses;
                return statuses;
            }
        }

        public Task NewTask
        {
            get { return GetValue<Task>(NewTaskProperty); }
            set { SetValue(NewTaskProperty, value); }
        }
        public static readonly PropertyData NewTaskProperty = RegisterProperty("NewTask", typeof(Task), null);

        /// <summary>
        /// Команда добавления новой задачи в БД
        /// </summary>
        public Command SaveNewTask
        {
            get
            {
                return new Command(() => 
                {
                    //SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
                    //try
                    //{
                    //    connection.Open();
                    //    SqlCommand command = new SqlCommand("SELECT * FROM Statuses", connection);
                    //    SqlDataReader reader = command.ExecuteReader();
                    //    while (reader.Read())
                    //    {
                    //        Status status = new Status()
                    //        {
                    //            Id = (int)reader[0],
                    //            StatusValue = reader[1].ToString(),
                    //        };
                    //        statuses.Add(status);
                    //    }
                    //    reader.Close();
                    //}

                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
                    //finally
                    //{
                    //    connection.Close();
                    //}
                });
            }
        }

    }
}
