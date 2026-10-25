using Domain.Entities;
using Domain.Interfaces;
using Domain.Enums;


namespace Application.Carts
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _repo;

        public CartService(ICartRepo repo)
        {
            _repo = repo;
        }

        public async Task<int> CreateCart(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            var cart = await GetCurrentCartByUserIdAsync(userId);


            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    TotalAmount = 0,
                    Status = enCartStatus.Active
                };

                cart = await _repo.AddAsync(cart);
            }

            return cart.Id;
        }

        public async Task<Cart?> GetCurrentCartByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            return await _repo.GetCurrentCartByUserIdAsync(userId);
        }

        public async Task MarkCartAsOrdered(int userId)
        {
            var cart = await GetCurrentCartByUserIdAsync(userId);

            if (cart == null)
                throw new KeyNotFoundException($"No active cart found for user with Id {userId}.");

            cart.Status = enCartStatus.Ordered;

            await _repo.UpdateAsync(cart);
        }

        
      
    }
}
