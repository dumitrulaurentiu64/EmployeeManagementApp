using EmpAPI.Dtos;
using EmpAPI.Models;

namespace EmpAPI.Repository
{
    public interface IAuthRepository
    {
        public User? Login(LoginDto dto);
        public User? GetUser(int userId);
        public void ChangePassword(string email, string newPassword);
        public void Register(RegisterDto dto, int parentId);
    }
}
