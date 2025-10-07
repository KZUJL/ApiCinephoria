namespace ApiCinephoria.Models
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public bool MustChangePassword { get; set; }

        public RoleModel Role { get; set; }
        public string Token { get; internal set; }
    }
}
