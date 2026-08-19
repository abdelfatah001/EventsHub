namespace Domain.Entities
{
    public class EmailSettings
    {
        public int Port { get; set; }
        public string Host { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string SenderName { get; set; } = null!;
    }
}
