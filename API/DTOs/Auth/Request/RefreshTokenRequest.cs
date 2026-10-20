namespace API.DTOs.Auth.Request
{
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }

        public string Email { get; set; }
    }
}
