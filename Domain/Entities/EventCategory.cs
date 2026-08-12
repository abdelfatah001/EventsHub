using System.Collections;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class EventCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }


        public ICollection<Event> Events { get; set; } = null!;

    }
}
