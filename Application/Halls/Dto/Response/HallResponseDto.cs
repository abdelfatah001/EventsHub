namespace Application.Halls.Dto.Response
{
    public class HallResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}
