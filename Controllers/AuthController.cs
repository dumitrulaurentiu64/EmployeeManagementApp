using EmpAPI.Dtos;
using EmpAPI.Helpers;
using EmpAPI.Models;
using EmpAPI.Repository;
using EmpAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EmpAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private const string JWT = "jwt";
        private readonly IAuthRepository _authRepository;
        private JwtService _jwtService;

        public AuthController(IAuthRepository authRepository, JwtService jwtService)
        {
            _authRepository = authRepository;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            User? user = _authRepository.Login(dto);

            if (user == null) return BadRequest(new { message = "Invalid Credentials" });

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var jwt = _jwtService.Generate(user.Id);

            Response.Cookies.Append(JWT, jwt, new CookieOptions
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
                var jwt = Request.Cookies[JWT];
                var token = _jwtService.Verify(jwt);
                int userId = int.Parse(token.Issuer);
                User? user = _authRepository.GetUser(userId);
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
            Response.Cookies.Delete(JWT);

            return Ok(new
            {
                message = "success"
            });
        }

        [HttpPut("changepass")]
        public IActionResult ChangePass(PassChangeDto dto)
        {
            if (dto.Email != null && dto.NewPassword != null)
            {
                try
                {
                    _authRepository.ChangePassword(dto.Email, dto.NewPassword);
                }
                catch (Exception)
                {
                    return BadRequest(new
                    {
                        message = "Failed to change password!"
                    });
                }
                return Ok(new
                {
                    message = "success"
                });
            }
            return BadRequest(new
            {
                message = "Failed to change password due to invalid values!!"
            });
        }
    }
}
