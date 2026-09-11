using Application.Tickets.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;

namespace Application.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _repo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly ITicketGenerator _ticketGenerator;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepo repo, ITicketTypeRepo ticketTypeRepo, ITicketGenerator ticketGenerator, IMapper mapper)
        {
            _repo = repo;
            _ticketTypeRepo = ticketTypeRepo;
            _ticketGenerator = ticketGenerator;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TicketResponseDto>?> AddTicketsAsync(int ticketTypeId, int orderItemId, int Amount)
        { // it doesn't add tickets to database, it just generate and tracked them as added
            if (ticketTypeId <= 0)
                throw new ArgumentOutOfRangeException("Ticket type id must be a positive integer.");

            if (orderItemId <= 0)
                throw new ArgumentOutOfRangeException("Order item id must be a positive integer.");

            if (Amount <= 0)
                throw new ArgumentOutOfRangeException("Amount must be a positive integer.");

            ICollection<Ticket> tickets = new List<Ticket>();

            for (int i = 0; i < Amount; i++)
            {
                var ticket = await _ticketGenerator.GenerateTicket(ticketTypeId, orderItemId);

                tickets.Add(ticket);
            }
            var ticketType = await _ticketTypeRepo.GetByIdAsync(ticketTypeId);

            var isAdded = await _repo.AddRangeAsync(tickets);

            if (!isAdded)
                return null;

            ICollection<TicketResponseDto> issuedtickets = new List<TicketResponseDto>();

            foreach (var ticket in tickets)
            {
                var issuedTicket = new TicketResponseDto
                {
                    TicketCode = ticket.TicketCode,
                    Price = ticketType.Price,
                    TicketTypeName = ticketType.Name,
                    IssuedAt = ticket.IssuedAt
                };

                issuedtickets.Add(issuedTicket);
            }

            return issuedtickets;
        }


        public async Task<IEnumerable<TicketDetailedResponseDto>> GetAllByOrderItemIdAsync(int orderItemId)
        {
            if (orderItemId <= 0)
                throw new ArgumentOutOfRangeException(nameof(orderItemId), orderItemId, "Order item id must be a positive integer.");

            var tickets = await _repo.GetAllByOrderItemIdAsync(orderItemId);

            return _mapper.Map<IEnumerable<TicketDetailedResponseDto>>(tickets);
        }

        public async Task<IEnumerable<TicketDetailedResponseDto>> GetAllByTicketTypeIdAsync(int ticketTypeId)
        {
            if (ticketTypeId <= 0)
                throw new ArgumentOutOfRangeException(nameof(ticketTypeId), ticketTypeId, "Ticket type id must be a positive integer.");

            var tickets = await _repo.GetAllByTicketTypeIdAsync(ticketTypeId);

            return _mapper.Map<IEnumerable<TicketDetailedResponseDto>>(tickets);
        }

        public async Task<TicketDetailedResponseDto> GetByIdAsync(int ticketId)
        {
            if (ticketId <= 0)
                throw new ArgumentOutOfRangeException(nameof(ticketId), ticketId, "Ticket id must be a positive integer.");

            var ticket = await _repo.GetByIdAsync(ticketId);

            return _mapper.Map<TicketDetailedResponseDto>(ticket);
        }   
        
        
        public async Task<TicketDetailedResponseDto?> GetByTicketCodeAsync(string ticketCode)
        {
            if (ticketCode is null)
                throw new ArgumentNullException(ticketCode, "Ticket code can't be null.");

            var ticket = await _repo.GetByTicketCodeAsync(ticketCode);

            return _mapper.Map<TicketDetailedResponseDto>(ticket);
        }

        public async Task<bool> DeactivateTicket(int ticketId)
        {
            if (ticketId <= 0)
                throw new ArgumentOutOfRangeException(nameof(ticketId), ticketId, "Ticket id must be a positive integer.");

            var ticket = await _repo.GetByIdAsync(ticketId);

            if (ticket is null)
                throw new KeyNotFoundException($"ticket with ID {ticketId} does not exist.");

            ticket.Status = enTicketStatus.Used;

            return await _repo.UpdateAsync(ticket);
        }

     
    }
}
