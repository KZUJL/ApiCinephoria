namespace ApiCinephoria.Models
{
    public class MailjetSettings
    {
        public string ApiKey { get; set; } = null!;
        public string ApiSecret { get; set; } = null!;
        public string SenderEmail { get; set; } = null!;
        public string SenderName { get; set; } = null!;
    }
}
