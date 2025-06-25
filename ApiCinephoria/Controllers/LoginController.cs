using ApiCinephoria.Data;
using ApiCinephoria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly CinephoriaContext _context;
        private readonly MailService _mailService;
        private string GenerateRandomPassword(int length = 10)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%&*";
            var res = new char[length];
            var rnd = new Random();
            for (int i = 0; i < length; i++)
                res[i] = valid[rnd.Next(valid.Length)];
            return new string(res);
        }
        public LoginController(CinephoriaContext context, MailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<LoginCreateModel>>> GetLoginById(int id)
        {
            var login = await _context.Users
                .Where(mt => mt.RoleId == id)
                .ToListAsync();

            return Ok(login);
        }

        [HttpGet("check-email")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            return Ok(exists);
        }

        [HttpPost]
        public async Task<ActionResult<LoginCreateModel>> CreateUser(LoginCreateModel user)
        {
            // Garde le mot de passe en clair temporairement pour l'email
            string plainPassword = user.Password;

            // Hash du mot de passe
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine("👤 Utilisateur enregistré, tentative d'envoi de mail...");
            // Envoi du mail via MailService
            bool mailSent = await _mailService.SendRegisterEmail(user.Email, user.UserName, plainPassword);

            if (!mailSent)
            {
                // Ici tu peux logger un warning ou gérer le cas d'erreur d'envoi mail
                // Mais la création utilisateur est validée même si le mail échoue
            }

            // Ne pas retourner le mot de passe
            user.Password = null;
            return CreatedAtAction(nameof(GetLoginById), new { id = user.UserId }, user);
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<LoginResponseDto>> Authenticate([FromBody] LoginRequestDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return Unauthorized("Email ou mot de passe incorrect.");
            }

            if (user.MustChangePassword)
            {
                return Ok(new
                {
                    MustChangePassword = true,
                    Message = "Le mot de passe doit être changé."
                });
            }

            var response = new LoginResponseDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName,
                Role = user.Role,
                MustChangePassword = user.MustChangePassword
            };

            return Ok(response);
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto dto)
        {
            if (dto == null || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.NewPassword))
                return BadRequest("Email et nouveau mot de passe requis.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return NotFound("Utilisateur non trouvé.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.MustChangePassword = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("send-reset-password")]
        public async Task<IActionResult> SendResetPassword([FromBody] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email requis.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("Utilisateur non trouvé.");

            // Génère un nouveau mot de passe
            string newPassword = GenerateRandomPassword();

            // Met à jour le mot de passe et le flag
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.MustChangePassword = true;
            await _context.SaveChangesAsync();

            // Envoie le mail
            bool mailSent = await _mailService.SendResetPasswordEmail(user.Email, user.UserName, newPassword);

            if (!mailSent)
                return StatusCode(500, "Erreur lors de l'envoi du mail.");

            return Ok(new { message = "Nouveau mot de passe généré et envoyé par mail." });
        }

    }
}
