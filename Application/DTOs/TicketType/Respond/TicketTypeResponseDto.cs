namespace Application.DTOs.TicketType.Respond
{
    public class TicketTypeResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int Price { get; set; }
        public int AvailableQuantity { get; set; }


    }
}
