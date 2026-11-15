namespace Domain.Common.Dto
{
    public class CartItemDto
    {
        public int TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
