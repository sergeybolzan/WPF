using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows;

namespace TaskListApp2.Models
{
    public static class TaskRepository
    {
        private static ObservableCollection<Task> tasks;

        public static ObservableCollection<Task> Tasks
        {
            get 
            {
                if (tasks == null) tasks = GenerateTaskRepository();
                return tasks; 
            }
        }
        
        private static ObservableCollection<Task> GenerateTaskRepository()
        {
            ObservableCollection<Task> tasks = new ObservableCollection<Task>();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT Tasks.Id, Name, Workload, BeginDate, EndDate, Statuses.Status, Persons.LastName + ' ' + Persons.FirstName + ' ' + Persons.MiddleName FROM Tasks INNER JOIN Statuses ON Tasks.StatusId = Statuses.Id INNER JOIN Persons ON Tasks.PersonId = Persons.Id", connection);
                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    Task task = new Task()
                    {
                        Id = (int)reader[0],
                        Name = reader[1].ToString(),
                        Workload = (int)reader[2],
                        BeginDate = (DateTime)reader[3],
                        EndDate = (DateTime)reader[4],
                        Status = reader[5].ToString(),
                        Person = reader[6].ToString()
                    };
                    tasks.Add(task);
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            } 
            return tasks;
        }
    }
}
