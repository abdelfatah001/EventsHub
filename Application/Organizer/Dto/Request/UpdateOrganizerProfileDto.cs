namespace Application.Organizers.Dto.Request
{
    public class UpdateOrganizerProfileDto
    {
        public string OrganizationName { get; set; } = null!;

        public string? Description { get; set; }

    }
}
