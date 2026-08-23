using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
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
