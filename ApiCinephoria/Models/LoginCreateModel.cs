using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCinephoria.Models
{
    public class LoginCreateModel
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public bool MustChangePassword { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public RoleModel? Role { get; set; }


    }
}
