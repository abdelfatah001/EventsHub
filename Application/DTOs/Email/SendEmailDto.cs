namespace Application.DTOs.Email
{
    public class SendEmailDto
    {
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string TextBody { get; set; } = null!;
        public string HtmlBody { get; set; } = null!;

    }
}
