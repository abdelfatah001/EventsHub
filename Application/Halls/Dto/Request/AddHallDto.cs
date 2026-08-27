namespace Application.Halls.Dto.Request
{
    public class AddHallDto
    {
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}
