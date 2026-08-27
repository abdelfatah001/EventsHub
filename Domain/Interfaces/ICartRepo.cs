using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICartRepo
    {
        Task<Cart?> AddAsync (Cart cart);
        Task<Cart?> GetCurrentCartByUserIdAsync (int userId);
        Task<Cart?> GetByIdAsync(int cartId);
    }
}
