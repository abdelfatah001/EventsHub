using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace Infrastructure.Repositories
{
    public class TicketRepo : ITicketRepo
    {
        private readonly AppDbContext _context;

        public TicketRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Ticket> tickets)
        {
            ArgumentNullException.ThrowIfNull(tickets);

            await _context.Tickets.AddRangeAsync(tickets);

            return true;
        }

        public async Task<IEnumerable<Ticket>> GetAllByOrderItemIdAsync(int orderItemId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(orderItemId);

            return await _context.Tickets
                .AsNoTracking()
                .Where(x => x.OrderItemId == orderItemId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetAllByTicketTypeIdAsync(int ticketTypeId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketTypeId);

            return await _context.Tickets
                .AsNoTracking()
                .Where(x => x.TicketTypeId == ticketTypeId)
                .ToListAsync();
        }

        public async Task<Ticket?> GetByIdAsync(int ticketId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticketId);

            return await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == ticketId);
        }        

        public async Task<Ticket?> GetByTicketCodeAsync(string ticketCode)
        {
            ArgumentException.ThrowIfNullOrEmpty(ticketCode);

            return await _context.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.TicketCode == ticketCode);
        }

        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticket.Id);

            _context.Tickets.Update(ticket);

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            ArgumentException.ThrowIfNullOrEmpty(code);

            return !await _context.Tickets
               .AsNoTracking()
               .AnyAsync(x => x.TicketCode == code);
        }

        
    }
}
