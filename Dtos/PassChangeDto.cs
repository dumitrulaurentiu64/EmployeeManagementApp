namespace EmpAPI.Dtos
{
    public class PassChangeDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
