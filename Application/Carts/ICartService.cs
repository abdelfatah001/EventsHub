using Domain.Entities;

namespace Application.Carts
{
    public interface ICartService
    {
        // if no existed cart to this user
        Task<int> AddCart (int UserId);
        Task<Cart?> GetCurrentCartByUserIdAsync(int userId);
        Task MarkCartAsOrdered (int cartId);

        Task UpdateTotalAmountAsync (int cartId, int totalAmount);

    }
}
