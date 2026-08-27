namespace Application.Events.Dto.Request
{
    public class AddEventDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int CategoryId { get; set; }
        public int OrganizerId { get; set; }


    }
}
