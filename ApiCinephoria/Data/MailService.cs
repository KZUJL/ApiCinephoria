using ApiCinephoria.Models;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
namespace ApiCinephoria.Data
{
    public class MailService
    {
        private readonly MailjetSettings _mailjetSettings;
        public MailService(IOptions<MailjetSettings> mailjetOptions)
        {
            _mailjetSettings = mailjetOptions.Value;
        }
        public async Task<bool> SendRegisterEmail(string toEmail, string username, string password)
        {
            MailjetClient client = new MailjetClient(_mailjetSettings.ApiKey, _mailjetSettings.ApiSecret);
          

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "cinephoria@hotmail.com")
            .Property(Send.FromName, "Cinephoria")
            .Property(Send.Subject, "Confirmation de votre inscription")
            .Property(Send.HtmlPart, $@"
                <h3>Bienvenue sur Cinephoria !</h3>
                <p>Merci pour votre inscription.</p>
                <p><strong>Nom d'utilisateur :</strong> {username}</p>
                <br/>
                <p>Nous vous souhaitons une bonne expérience 🎬</p>")
            .Property(Send.Recipients, new JArray {
                new JObject {
                    { "Email", toEmail },
                    { "Name", username }
                }
            });

            MailjetResponse response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Échec d'envoi du mail !");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"ErrorInfo: {response.GetErrorInfo()}");
                Console.WriteLine($"ErrorMessage: {response.GetErrorMessage()}");
                Console.WriteLine($"RawResponse: {response.Content}"); 
            }
            else
            {
                Console.WriteLine("✅ Email envoyé avec succès !");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SendResetPasswordEmail(string toEmail, string username, string newPassword)
        {
            MailjetClient client = new MailjetClient(_mailjetSettings.ApiKey, _mailjetSettings.ApiSecret);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "cinephoria@hotmail.com")
            .Property(Send.FromName, "Cinephoria")
            .Property(Send.Subject, "Réinitialisation de votre mot de passe Cinephoria")
            .Property(Send.HtmlPart, $@"
                <h3>Réinitialisation de votre mot de passe</h3>
                <p>Bonjour {username},</p>
                <p>Votre nouveau mot de passe temporaire est : <strong>{newPassword}</strong></p>
                <p>Merci de le changer dès votre prochaine connexion.</p>
                <br/>
                <p>L'équipe Cinephoria</p>")
            .Property(Send.Recipients, new JArray {
        new JObject {
            { "Email", toEmail },
            { "Name", username }
        }
            });

            MailjetResponse response = await client.PostAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Échec d'envoi du mail de réinitialisation !");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"ErrorInfo: {response.GetErrorInfo()}");
                Console.WriteLine($"ErrorMessage: {response.GetErrorMessage()}");
                Console.WriteLine($"RawResponse: {response.Content}");
            }
            else
            {
                Console.WriteLine("✅ Email de réinitialisation envoyé avec succès !");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SendContactEmail(string fromEmail, string username, string subject, string message)
        {
            MailjetClient client = new MailjetClient(_mailjetSettings.ApiKey, _mailjetSettings.ApiSecret);

            // 📩 1. Envoi à Cinephoria
            MailjetRequest adminRequest = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "cinephoria@hotmail.com")
            .Property(Send.FromName, "Cinephoria")
            .Property(Send.Subject, $"[Contact] {subject}")
            .Property(Send.HtmlPart, $@"
                <h3>Demande de contact reçue</h3>
                <p><strong>De :</strong> {username} ({fromEmail})</p>
                <p><strong>Sujet :</strong> {subject}</p>
                <p><strong>Message :</strong></p>
                <p>{message}</p>
                <br/>
                <p>Ce message a été envoyé via le formulaire de contact Cinephoria.</p>")
            .Property(Send.Recipients, new JArray {
        new JObject {
            { "Email", "cinephoria@hotmail.com" },
            { "Name", "Cinephoria" }
        }
            });

            MailjetResponse adminResponse = await client.PostAsync(adminRequest);

            if (!adminResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Échec d'envoi du mail à Cinephoria !");
                Console.WriteLine($"StatusCode: {adminResponse.StatusCode}");
                Console.WriteLine($"ErrorMessage: {adminResponse.GetErrorMessage()}");
                return false;
            }

            // 📬 2. Confirmation à l'utilisateur
            MailjetRequest userRequest = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, "cinephoria@hotmail.com")
            .Property(Send.FromName, "Cinephoria")
            .Property(Send.Subject, "Votre message a bien été reçu")
            .Property(Send.HtmlPart, $@"
                <p>Bonjour {username},</p>
                <p>Nous avons bien reçu votre message avec pour sujet : <strong>{subject}</strong>.</p>
                <p>Contenu du message :</p>
                <blockquote>{message}</blockquote>
                <p>Nous reviendrons vers vous dès que possible.</p>
                <br/>
                <p>Merci,<br/>L’équipe Cinephoria</p>")
            .Property(Send.Recipients, new JArray {
        new JObject {
            { "Email", fromEmail },
            { "Name", username }
        }
            });

            MailjetResponse userResponse = await client.PostAsync(userRequest);

            if (!userResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("❌ Échec d'envoi du mail à l'utilisateur !");
                Console.WriteLine($"StatusCode: {userResponse.StatusCode}");
                Console.WriteLine($"ErrorMessage: {userResponse.GetErrorMessage()}");
                return false;
            }

            Console.WriteLine("✅ Les deux e-mails ont été envoyés avec succès !");
            return true;
        }

    }
}
