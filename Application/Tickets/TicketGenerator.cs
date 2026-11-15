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

        public async Task<Ticket> GenerateTicket(int ticketTypeId, int orderItemId, int order)
        {

            var fixedCode = await GenerateTicketCode(ticketTypeId);
            return new Ticket
            {
                OrderItemId = orderItemId,
                TicketTypeId = ticketTypeId,
                TicketCode = fixedCode + " - " + order.ToString(),
                Status = enTicketStatus.Sold,
                IssuedAt = DateTime.Now
            };
        }

        private async Task<string> GenerateTicketCode(int ticketTypeId)
        {
            string ticketCode = "";

            while (true)
            {
                ticketCode = 
                    $"TKT- {ticketTypeId} - {DateTime.Today}";

                if (await _ticketRepo.IsCodeUniqueAsync(ticketCode))
                    return ticketCode;
            }
        }
    }
}
