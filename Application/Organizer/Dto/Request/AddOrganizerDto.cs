using Application.Users.Dto.Request;

namespace Application.Organizers.Dto.Request
{
    public class AddOrganizerDto
    {
        public AddUserDto userDto = new AddUserDto();
        public string OrganizationName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
