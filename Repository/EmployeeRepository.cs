using Dapper;
using EmpAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace EmpAPI.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbConnection db;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this.db = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon"));
        }
        public void Delete(int id)
        {
            this.db.Execute("DELETE FROM Employee WHERE EmployeeId = @Id", new { id });
        }

        public Employee Find(int id)
        {
            return this.db.Query<Employee>("SELECT * FROM employees where EmployeeId = @Id", new { id }).SingleOrDefault();
        }

        public List<Employee> GetAll()
        {
            return this.db.Query<Employee>("SELECT * FROM employees").ToList();
        }

        public Employee Insert(Employee employee)
        {
            string sql = "INSERT INTO Employee (Name, Lastname, Position, Department, DateOfJoining, PhotoFileName, BaseSalary, Increase, GrossPrizes, Deductions) VALUES (@Name, @Lastname, @Position, @Department, @DateOfJoining, @PhotoFileName, @BaseSalary, @Increase, @GrossPrizes, @Deductions); " +
                         "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Update(Employee employee)
        {
            string sql =
               "UPDATE Employee " +
               "SET Name = @Name, " +
               "Lastname = @Lastname, " +
               "Position = @Position, " +
               "Department = @Department, " +
               "DateOfJoining = @DateOfJoining, " +
               "PhotoFileName = @PhotoFileName, " +
               "BaseSalary = @BaseSalary, " +
               "Increase = @Increase, " +
               "GrossPrizes = @GrossPrizes, " +
               "Deductions = @Deductions " +
               "WHERE EmployeeId = @EmployeeId";
            this.db.Execute(sql, employee);
            return employee;
        }
    }
}
