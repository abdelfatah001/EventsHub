namespace API.DTOs.Auth.Request
{
    public class MyRefreshRequest
    {
        public string RefreshToken { get; set; }

        public string Email { get; set; }
    }
}
