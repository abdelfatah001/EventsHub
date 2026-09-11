using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartRepo
    {
        Task<Cart?> AddAsync (Cart cart);
        Task<Cart?> GetCurrentCartByUserIdAsync (int userId);
        Task<Cart?> GetByIdAsync(int cartId);
        
        // to mark cart as ordered
        Task<bool> UpdateAsync (Cart cart);

        Task<bool> UpdateTotalAmount (int cartId, int amount);

    }
}
