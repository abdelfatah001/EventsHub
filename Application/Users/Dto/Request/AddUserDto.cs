using Domain.Enums;


namespace Application.Users.Dto.Request
{
    public class AddUserDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!;
        public enUserRole Role { get; set; } = new enUserRole();

        public bool IsOrganizer { get; set; }
    }
}
