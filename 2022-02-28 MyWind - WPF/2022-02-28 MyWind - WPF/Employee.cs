using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace _2022_02_28_MyWind___WPF
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Notes { get; set; }

        public string FullName => $"{FirstName} {LastName.ToUpper()}".Trim();

        public Employee()
        { }

        public Employee(MySqlDataReader reader)
        {
            this.Id = (int)reader["id"];
            this.FirstName = reader["first_name"].ToString();
            this.LastName = reader["last_name"].ToString();
            this.Email = reader["email_address"].ToString();
            this.Notes = reader["notes"].ToString();
        }

        public Employee(Employee employee)
        {
            this.Id = employee.Id;
            this.FirstName = employee.FirstName;
            this.LastName = employee.LastName;
            this.Email = employee.Email;
            this.Notes = employee.Notes;

        }
    }
}
