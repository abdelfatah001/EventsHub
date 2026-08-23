using Domain.Interfaces;
using Infrastructure.DbContexts;

namespace Infrastructure.Repositories
{
    public class TicketTypeRepo : ITicketTypeRepo
    {
        private readonly AppDbContext _context;

        public TicketTypeRepo(AppDbContext context)
        {
            _context = context;
        }


    }
}
