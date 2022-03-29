using EmpAPI.Dtos;
using EmpAPI.Helpers;
using EmpAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace EmpAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, JwtService jwtService)
        {
            _configuration = configuration;
            _jwtService = jwtService;
        }
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            ////////////////////////////////////
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            string query = @"
                                insert into dbo.Users
                                (Name,Email,Password)                           
                                values 
                                (
                                    '" + user.Name + @"'
                                    ,'" + user.Email + @"'
                                    ,'" + user.Password + @"'
                                )
                                ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return Created("success", user);

            ///////////////////////////////////


            //return Created("success", _repository.Create(user));

            /////////////////////////////////////////////
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            /////////////////////////
            string query = @"select Id, Name, Email, Password
                    from dbo.Users";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            var convertedList = (from rw in table.AsEnumerable()
                                 select new User()
                                 {
                                     Id = Convert.ToInt32(rw["Id"]),
                                     Name = Convert.ToString(rw["Name"]),
                                     Password = Convert.ToString(rw["Password"]),
                                     Email = Convert.ToString(rw["Email"])
                                 }).ToList();

            var user = convertedList.FirstOrDefault(u => u.Email == dto.Email);
            /////////////////////////
            //var user = _repository.GetByEmail(dto.Email);
            ////////////////////////////
            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var jwt = Request.Cookies["jwt"];

                var token = _jwtService.Verify(jwt);

                int userId = int.Parse(token.Issuer);

                string query = @"select Id, Name, Email, Password, User_Role
                    from dbo.Users";
                DataTable table = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
                    }
                }

                var convertedList = (from rw in table.AsEnumerable()
                                     select new User()
                                     {
                                         Id = Convert.ToInt32(rw["Id"]),
                                         Name = Convert.ToString(rw["Name"]),
                                         Password = Convert.ToString(rw["Password"]),
                                         Email = Convert.ToString(rw["Email"]),
                                         User_Role = Convert.ToString(rw["User_Role"])
                                     }).ToList();

                var user = convertedList.FirstOrDefault(u => u.Id == userId);
                ///////////////////
                //var user = _repository.GetById(userId);
                //////////////////////////
                return Ok(user);
            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");


            return Ok(new
            {
                message = "success"
            });
        }
    }
}
