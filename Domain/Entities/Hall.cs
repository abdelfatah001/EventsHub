
namespace Domain.Entities
{
    public class Hall
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Capacity { get; set; }

        public ICollection<EventOccurence> Occurences { get; set; }
            = new List<EventOccurence>();
    }
}
