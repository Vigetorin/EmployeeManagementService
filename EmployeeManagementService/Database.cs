using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeManagementService
{
    public class Database(string connectionString) : IDatabase
    {
        const int maxBatchSize = 500;

        readonly string connectionString = connectionString;

        static string GetEmployeeInfo(IEmployee employee) => $"'{employee.Name}', '{employee.BirthDate:yyyy-MM-dd}', { (employee.Gender == Gender.Male ? 1 : 0)}";

        public void Create()
        {
            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(
                "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = 'Employees' AND xtype = 'U')" + 
                "CREATE TABLE Employees (" +
                    "id INT IDENTITY(1,1) NOT NULL," +
                    "name NVARCHAR(MAX) NOT NULL," +
                    "birth_date DATE NOT NULL," +
                    "gender BIT NOT NULL" +
                ")", connection);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void Add(IEmployee employee)
        {
            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(
                "INSERT INTO Employees (name, birth_date, gender) " +
               $"VALUES ({GetEmployeeInfo(employee)})", connection);
            connection.Open();
            command.ExecuteNonQuery();
        }
        public void Add(IEmployee[] employees)
        {
            using SqlConnection connection = new(connectionString);
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction();
            for (int batch = 0; batch < employees.Length; batch += maxBatchSize)
            {
                string text = "INSERT INTO Employees (name, birth_date, gender) VALUES ";
                int last = Math.Min(batch + maxBatchSize, employees.Length) - 1;
                for (int i = batch; i < last; i++)
                    text += $"({GetEmployeeInfo(employees[i])}), ";
                text += $"({GetEmployeeInfo(employees[last])})";
                using SqlCommand command = new(text, connection);
                command.Transaction = transaction;
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        public IEmployee[] GetAll()
        {
            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(
                "SELECT DISTINCT name, birth_date, gender " +
                "FROM Employees " +
                "ORDER BY name", connection);
            DataTable data = new();
            connection.Open();
            data.Load(command.ExecuteReader());
            IEmployee[] employees = [.. data.AsEnumerable().Select(row => new Employee((string)row[0], DateOnly.FromDateTime((DateTime)row[1]), (bool)row[2] ? Gender.Male : Gender.Female))];
            return employees;
        }
        public IEmployee[] Select(char firstLetter, Gender gender)
        {
            using SqlConnection connection = new(connectionString);
            using SqlCommand command = new(
                "SELECT DISTINCT name, birth_date, gender " +
                "FROM Employees " +
               $"WHERE name LIKE '{firstLetter}%' AND gender = {(gender == Gender.Male ? "1" : "0")} " +
                "ORDER BY name", connection);
            DataTable data = new();
            connection.Open();
            data.Load(command.ExecuteReader());
            IEmployee[] employees = [.. data.AsEnumerable().Select(row => new Employee((string)row[0], DateOnly.FromDateTime((DateTime)row[1]), (bool)row[2] ? Gender.Male : Gender.Female))];
            return employees;
        }
    }
}
