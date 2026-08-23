using Domain.Interfaces;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories
{
    public class CartItemRepo : ICartItemRepo
    {

        private readonly AppDbContext _context;

        public CartItemRepo (AppDbContext context)
        {
            _context = context;
        }


    }
}
