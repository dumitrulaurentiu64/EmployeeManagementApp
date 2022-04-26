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
            string sql = "INSERT INTO Employee (EmployeeName, Department, DateOfJoining, PhotoFileName) VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName); " +
                         "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.db.Query<int>(sql, employee).Single();
            employee.EmployeeId = id;
            return employee;
        }

        public Employee Update(Employee employee)
        {
            string sql =
               "UPDATE Employee " +
               "SET EmployeeName = @EmployeeName, " +
               "Department = @Department, " +
               "DateOfJoining = @DateOfJoining, " +
               "PhotoFileName = @PhotoFileName " +
               "WHERE EmployeeId = @EmployeeId";
            this.db.Execute(sql, employee);
            return employee;
        }
    }
}
