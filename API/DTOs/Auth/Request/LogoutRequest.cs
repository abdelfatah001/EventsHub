namespace API.DTOs.Auth.Request
{
    public class LogoutRequest
    {
        public string Email { get; set; }

        public string RefreshToken { get; set; }
    }
}
