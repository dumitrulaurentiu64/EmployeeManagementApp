using Dapper;
using EmpAPI.Dtos;
using EmpAPI.Helpers;
using EmpAPI.Models;
using System.Data;
using System.Data.SqlClient;

namespace EmpAPI.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IDbConnection db;
        private readonly IConfiguration _configuration;
        public AuthRepository(IConfiguration _configuration, JwtService jwtService)
        {
            this._configuration = _configuration;
            this.db = new SqlConnection(_configuration.GetConnectionString("EmployeeAppCon"));
        }
        public void ChangePassword(string? email, string? newPassword)
        {
            string password = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var parameters = new {Password = password, Email = email };

            string sql =
              "UPDATE Users " +
              "SET Password = @Password " +
              "WHERE Email = @Email";
            this.db.Execute(sql, parameters);
        }

        public User? GetUser(int userId)
        {
            string sql = "SELECT * FROM Users where Id = @userId";
            return this.db.Query<User>(sql, new { userId }).SingleOrDefault();
        }

        public User? Login(LoginDto dto)
        {
            User? user = this.db.Query<User>("SELECT * FROM Users where Email = @Email", new { dto.Email }).SingleOrDefault();

            return user;
        }
        public void Register(RegisterDto dto, int parentId)
        {
            var user = new User
            {
                Id = parentId,
                Firstname = dto.Firstname,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            string sql = "INSERT INTO Users (Id, Firstname, Email, Password) VALUES (@Id, @Firstname, @Email, @Password);";
            this.db.Query<int>(sql, user);
        }
    }
}
