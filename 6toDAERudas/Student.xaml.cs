using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _6toDAERudas
{
    /// <summary>
    /// Lógica de interacción para Student.xaml
    /// </summary>
    public partial class Student : Window
    {
        string connectionString = "Data Source=LAB1504-28\\SQLEXPRESS;Initial Catalog=SharonDB;User Id=userTecsup;Password=Tecsup";

        public Student()
        {
            InitializeComponent();
        }
        private void Button_ListaDatatable_Click(object sender, RoutedEventArgs e)
        {
            List<Estudiante> estudiantes = GetEstudiantesFromDataTable();
            dataGrid.ItemsSource = estudiantes;
        }
        private void Button_ListObject_Click(object sender, RoutedEventArgs e)
        {
            List<Estudiante> estudiantes = GetEstudiantesFromListObject();
            dataGrid.ItemsSource = estudiantes;
        }
        private void Button_Buscar_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtBuscar.Text;
            List<Estudiante> students = SearchStudentsByName(searchText);
            dataGrid.ItemsSource = students;
        }
        private List<Estudiante> GetEstudiantesFromDataTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                List<Estudiante> estudiantes = new List<Estudiante>();
                foreach (DataRow row in dataTable.Rows)
                {
                    estudiantes.Add(new Estudiante
                    {
                        StudentId = Convert.ToInt32(row["StudenId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }

                return estudiantes;
            }
        }

        private List<Estudiante> GetEstudiantesFromListObject()
        {
            List<Estudiante> estudiantes = new List<Estudiante>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Students", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Estudiante estudiante = new Estudiante
                        {
                            StudentId = Convert.ToInt32(reader["StudenId"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        };
                        estudiantes.Add(estudiante);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener estudiantes desde la base de datos: " + ex.Message);
            }

            return estudiantes;
        }
        private List<Estudiante> SearchStudentsByName(string name)
        {
            List<Estudiante> students = new List<Estudiante>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Students WHERE FirstName LIKE @Name OR LastName LIKE @Name", connection);
                command.Parameters.AddWithValue("@Name", "%" + name + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    students.Add(new Estudiante
                    {
                        StudentId = Convert.ToInt32(row["StudenId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    });
                }
            }

            return students;
        }

    }
    public class Estudiante 
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

}

