using Dapper;
using EmpAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace EmpAPI.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private IDbConnection db;
        private readonly IConfiguration _configuration;

        public DepartmentRepository(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this.db = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon"));
        }

        public void Delete(int id)
        {
            this.db.Execute("DELETE FROM Department WHERE DepartmentId = @Id", new { id });
        }

        public Department Find()
        {
            throw new NotImplementedException();
        }

        public List<Department> GetAll()
        {
            return this.db.Query<Department>("SELECT * FROM Department").ToList();
        }

        public Department Insert(Department department)
        {
            string sql = "INSERT INTO Department (DepartmentName) VALUES (@DepartmentName); " + 
                         "SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = this.db.Query<int>(sql, department).Single();
            department.DepartmentId = id;
            return department;
        }

        public Department Update(Department department)
        {
            string sql =
               "UPDATE Department " +
               "SET DepartmentName = @DepartmentName " +
               "WHERE DepartmentId = @DepartmentId";
            this.db.Execute(sql, department);
            return department;
        }
    }
}
