namespace Application.DTOs.Events.Request
{
    public class UpdateEventDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int CategoryId { get; set; }

    }
}
