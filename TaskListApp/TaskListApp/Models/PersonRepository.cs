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
    public static class PersonRepository
    {
        private static ObservableCollection<Person> persons;

        public static ObservableCollection<Person> Persons
        {
            get
            {
                if (persons == null) persons = GeneratePersonRepository();
                return persons;
            }
        }

        private static ObservableCollection<Person> GeneratePersonRepository()
        {
            ObservableCollection<Person> persons = new ObservableCollection<Person>();
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Persons", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Person person = new Person()
                    {
                        Id = (int)reader[0],
                        LastName = reader[1].ToString(),
                        FirstName = reader[2].ToString(),
                        MiddleName = reader[3].ToString(),
                    };
                    persons.Add(person);
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
            return persons;
        }
    }
}
