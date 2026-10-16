using Domain.Common;

namespace Domain.Entities
{
    public class Organizer : IAuditable
    {
        public int OrganizerId { get; set; }

        public int UserId { get; set; }

        public string? OrganizationName { get; set; } 

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public User User { get; set; } = null!;

        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
