using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartItemRepo
    {
        Task<CartItem?> AddAsync (CartItem item);
        Task<CartItem?> GetById (int cartItemId);
        Task<IEnumerable<CartItem>> GetByCartId (int cartId);
        Task<bool> UpdateAsync (CartItem cartItem);
        Task<bool> DeleteAsync (CartItem cartItem);
    }
}
