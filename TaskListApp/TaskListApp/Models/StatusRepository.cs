using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskListApp2.Models
{
    public static class StatusRepository
    {
        private static ObservableCollection<Status> statuses;

        public static ObservableCollection<Status> Statuses
        {
            get
            {
                if (statuses == null) statuses = GenerateStatusRepository();
                return statuses;
            }
        }

        private static ObservableCollection<Status> GenerateStatusRepository()
        {
            ObservableCollection<Status> statuses = new ObservableCollection<Status>();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Statuses", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Status status = new Status()
                    {
                        Id = (int)reader[0],
                        StatusValue = reader[1].ToString(),
                    };
                    statuses.Add(status);
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
            return statuses;
        }

    }
}
