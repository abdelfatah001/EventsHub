using Domain.Common.Dto;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartItemRepo
    {
        Task<CartItem?> AddAsync (CartItem item);
        Task<CartItem?> GetById (int cartItemId);
        Task<IEnumerable<CartItemDto>> GetItemsByCartId (int cartId);
        Task<bool> UpdateAsync (CartItem cartItem);
        Task<bool> DeleteAsync (CartItem cartItem);

        Task<int> CalculateCartTotalAmount (int cartId);

        // if cart item is repeated in the same cart return its Id
        Task<int> IsCartItemRepeated(int cartId, int TicketTypeId);
    }
}
