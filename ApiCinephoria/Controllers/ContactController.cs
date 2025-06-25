using ApiCinephoria.Data;
using Microsoft.AspNetCore.Mvc;
using ApiCinephoria.Models;

namespace ApiCinephoria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly MailService _mailService;

        public ContactController(MailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("contact")]
        public async Task<IActionResult> SendContact([FromBody] ContactRequestDto dto)
        {
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Subject) ||
                string.IsNullOrWhiteSpace(dto.Message))
               
                return BadRequest("Tous les champs sont requis.");

            // Si Username est null ou vide, on passe une valeur par défaut
            string username = string.IsNullOrWhiteSpace(dto.Username) ? "Utilisateur inconnu" : dto.Username;
            bool mailSent = await _mailService.SendContactEmail(dto.Email, dto.Username, dto.Subject, dto.Message);

            if (!mailSent)
                return StatusCode(500, "Erreur lors de l'envoi du mail de contact.");

            return Ok(new { message = "Votre message a bien été envoyé." });
        }
    }      
}
