namespace API.DTOs.Auth.Request
{
    public class VerifyEmailDto
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
    }
}
