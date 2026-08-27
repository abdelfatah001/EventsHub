using Application.Tickets.Dto.Request;
using Application.Tickets.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepo _repo;
        private readonly ITicketTypeRepo _typeRepo;
        private readonly ITicketGenerator _ticketGenerator;
        private readonly IMapper _mapper;
        public TicketService(ITicketRepo repo, ITicketTypeRepo ticketTypeRepo, ITicketGenerator ticketGenerator, IMapper mapper)
        {
            _repo = repo;
            _typeRepo = ticketTypeRepo;
            _ticketGenerator = ticketGenerator;
            _mapper = mapper;
        }

        public async Task<TicketResponseDto> AddAsync(IssueTicketDto issuedTicket)
        {
            if (issuedTicket is null)
                throw new ArgumentNullException(nameof(issuedTicket), "Issue Ticket payload cannot be null.");

            if (!await _typeRepo.IsThereAvaliableTicketsAsync(issuedTicket.TicketTypeId))
                throw new TicketUnavailableException("There are no available tickets of this type.");

            var ticket = await _ticketGenerator.GenerateTicket(issuedTicket);

            ticket = await _repo.AddAsync(ticket);

            return _mapper.Map<TicketResponseDto>(ticket);
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
