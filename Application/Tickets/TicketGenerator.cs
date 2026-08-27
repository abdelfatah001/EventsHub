using Application.Tickets.Dto.Request;
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

        public async Task<Ticket> GenerateTicket(IssueTicketDto dto)
        {
            return new Ticket
            {
                OrderItemId = dto.OrderItemId,
                TicketTypeId = dto.TicketTypeId,
                TicketCode = await GenerateTicketCode(),
                Status = enTicketStatus.Active,
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
