using Domain.Interfaces;

namespace Application.Carts
{
    public class CartService : ICartService
    {
        private readonly ICartRepo _repo;

        public CartService(ICartRepo repo)
        {
            _repo = repo;
        }


    }
}
