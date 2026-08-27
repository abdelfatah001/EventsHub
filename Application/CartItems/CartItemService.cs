using Domain.Interfaces;

namespace Application.CartItems
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepo _repo;
        public CartItemService(ICartItemRepo repo) 
        {
            _repo = repo;
        }
    }
}
