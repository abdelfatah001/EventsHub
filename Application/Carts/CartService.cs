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

        public async Task<int> AddCart(int userId)
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

        public async Task MarkCartAsOrdered(int cartId)
        {
            if (cartId <= 0) 
                throw new ArgumentOutOfRangeException("Cart Id must be a positive integer.");

            var cart = await _repo.GetByIdAsync(cartId);

            if (cart is null)
                throw new KeyNotFoundException($"Cart with Id {cartId} does not exist");

            cart.Status = enCartStatus.Ordered;

            await _repo.UpdateAsync(cart);
        }

        public Task UpdateTotalAmountAsync(int cartId, int totalAmount)
        {
            if (cartId <= 0)
                throw new ArgumentOutOfRangeException("Cart Id must be a positive integer.");

            if (totalAmount <= 0)
                throw new ArgumentOutOfRangeException();



        }
    }
}
