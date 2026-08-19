namespace Application.DTOs.Events.Response
{
    public class EventResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int categoryId { get; set; }
        public int OrganizerId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
