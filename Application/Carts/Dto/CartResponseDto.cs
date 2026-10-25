

using Application.CartItems.Dto.Response;

namespace Application.Carts.Dto
{
    public class CartResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public IEnumerable<ItemResponseDto> CartItems { get; set; } = new List<ItemResponseDto>();
        public double TotalAmount { get; set; }
    }
}
