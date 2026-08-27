namespace Application.Users.Dto.Request
{
    public class UpdateProfileDto 
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
    }
}
