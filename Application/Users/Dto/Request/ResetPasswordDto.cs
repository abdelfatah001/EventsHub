namespace Application.Users.Dto.Request
{
    public class ResetPasswordDto
    {
        public string ResetToken { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}
