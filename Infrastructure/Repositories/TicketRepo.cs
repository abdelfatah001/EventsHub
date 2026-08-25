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

        public async Task<Ticket?> AddAsync(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);

            _context.Tickets.Add(ticket);

            return await _context.SaveChangesAsync() > 0 ? ticket : null;
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

        public async Task<bool> UpdateAsync(Ticket ticket)
        {
            ArgumentNullException.ThrowIfNull(ticket);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(ticket.Id);

            _context.Tickets.Update(ticket);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
