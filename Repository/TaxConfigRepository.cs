using Dapper;
using EmpAPI.Dtos;
using System.Data;
using System.Data.SqlClient;

namespace EmpAPI.Repository
{
    public class TaxConfigRepository : ITaxConfigRepository
    {
        private IDbConnection db;
        private readonly IConfiguration _configuration;

        public TaxConfigRepository(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            this.db = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon"));
        }

        public void UpdateTaxes(TaxDto taxDto)
        {
            string sql =
               "UPDATE Taxes " +
               "SET Tax = @Tax, " +
               "CAS = @CAS, " +
               "CASS = @CASS;";
            this.db.Execute(sql, taxDto);
        }

        public void InsertTaxes(TaxDto taxDto)
        {
            string sql = "INSERT INTO Taxes (Tax, CAS, CASS, Pass) VALUES (@Tax, @CAS, @CASS, @Pass);";
            this.db.Execute(sql, taxDto);
        }

        public TaxDto Get()
        {
            return this.db.Query<TaxDto>("SELECT * FROM Taxes").SingleOrDefault();
        }


    }
}
