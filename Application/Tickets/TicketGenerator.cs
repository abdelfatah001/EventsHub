using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Application.Tickets
{
    public class TicketGenerator : ITicketGenerator
    {
        private readonly ITicketRepo _ticketRepo;

        public TicketGenerator(ITicketRepo ticketRepo)
        {
            _ticketRepo = ticketRepo;
        }

        public async Task<Ticket> GenerateTicket(int ticketTypeId, int orderItemId)
        {
            return new Ticket
            {
                OrderItemId = orderItemId,
                TicketTypeId = ticketTypeId,
                TicketCode = await GenerateTicketCode(),
                Status = enTicketStatus.Sold,
                IssuedAt = DateTime.Now
            };
        }

        private async Task<string> GenerateTicketCode()
        {
            string ticketCode = "";

            while (true)
            {
                ticketCode = 
                    $"TKT-{RandomNumberGenerator.GetHexString(8)}";

                if (await _ticketRepo.IsCodeUniqueAsync(ticketCode))
                    return ticketCode;
            }
        }
    }
}
