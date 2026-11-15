using Domain.Common.Dto;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartItemRepo
    {
        Task<CartItem?> AddAsync (CartItem item);
        Task<CartItem?> GetByIdAsync (int cartItemId);
        Task<IEnumerable<CartItemDto>> GetItemsByCartId (int cartId);
        Task<bool> UpdateAsync (CartItem cartItem);
        Task<bool> DeleteAsync (CartItem cartItem);

        Task<decimal> CalculateCartTotalPrice (int cartId);

        // if cart item is repeated in the same cart return its Id
        Task<CartItem?> IsCartItemRepeated(int cartId, int TicketTypeId);
    }
}
