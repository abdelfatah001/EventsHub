using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Event : IAuditable
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public int OrganizerId { get; set; }
        public User? Organizer { get; set; }

        public int CategoryId { get; set; }
        public EventCategory? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public ICollection<EventOccurence> Occurences { get; set; }
            = new List<EventOccurence>();

    }
}
