using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EmpAPI.Models
{
    public class User
    {
        public int? Id { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }

        public string UserRole { get; set; }
        [JsonIgnore] public string Password { get; set; }    

    }
}
