using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2022_02_28_MyWind___WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //string connectionString = "User Id=root;Host=localhost;Database=northwind";
        string connectionString = "Uid=root;Host=localhost;Database=northwind";
        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var con = new MySqlConnection(connectionString))
            {
                con.Open();
                var sql = "select id, " +
                                 "first_name, " +
                                 "last_name, " +
                                 "email_address, " +
                                 "notes " +
                          " from employees";
                using(var cmd = new MySqlCommand(sql, con))
                    using (var reader = cmd.ExecuteReader())
                        while (reader.Read())
                            employees.Add(new Employee(reader));
                
                dgList.ItemsSource = employees;
                isEditMode(false);
            }
        }

        private void isEditMode(bool isEditMode)
        {
            txtFirstName.IsReadOnly = !isEditMode;
            txtLastName.IsReadOnly = !isEditMode;
            txtEmail.IsReadOnly = !isEditMode;
            txtNotes.IsReadOnly = !isEditMode;

            dgList.IsEnabled = !isEditMode;
            pnlModify.Visibility = isEditMode ? Visibility.Hidden : Visibility.Visible; 
            pnlSave.Visibility = !isEditMode ? Visibility.Hidden : Visibility.Visible;
        }

        private void dgList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            grpData.DataContext = dgList.SelectedItem;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            grpData.DataContext = new Employee();
            isEditMode(true);            
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (grpData.DataContext != null)
            {
                //TODO: adott rekord adatbázisból lekérdezése -> változhatott az ablak megnyitása óta
                grpData.DataContext = new Employee( (Employee)grpData.DataContext );
                isEditMode(true);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            //TODO: kötelező mező ellenőrzés
            var employee = (Employee)grpData.DataContext;
            if (employee.Id == 0)
            {
                using (var con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    var sql = "insert into employees(first_name, last_name, email_address, notes) " +
                              "values (@first_name, @last_name, @email_address, @notes); " +
                              "select last_insert_id() as id";
                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@first_name", employee.FirstName);
                        cmd.Parameters.AddWithValue("@last_name", employee.LastName);
                        cmd.Parameters.AddWithValue("@email_address", employee.Email);
                        cmd.Parameters.AddWithValue("@notes", employee.Notes);

                        employee.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        employees.Add(employee);
                        dgList.SelectedItem = employee;
                        isEditMode(false);
                    }
                }
            }
            else
            {

            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            grpData.DataContext = null;
            isEditMode(false);
        }

        
    }
}
