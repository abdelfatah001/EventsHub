using Application.TicketTypes.Dto.Request;
using Application.TicketTypes.Dto.Respond;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.TicketTypes
{
    public class TicketTypeService : ITicketTypeService
    {
        private readonly ITicketTypeRepo _repo;
        private readonly IMapper _mapper;
        public TicketTypeService(ITicketTypeRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<TicketTypeDetailedResponseDto> AddAsync(AddTicketTypeDto newTicketType)
        {
            if (newTicketType is null)
                throw new ArgumentNullException(nameof(newTicketType), "Ticket Type add payload cannot be null.");

            var ticketType = await _repo.AddAsync(_mapper.Map<TicketType>(newTicketType));

            return _mapper.Map<TicketTypeDetailedResponseDto>(ticketType);
        }

        

        public async Task<TicketTypeResponseDto> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Ticket Type id must be a positive integer.");

            var ticketType = await _repo.GetByIdAsync(id);

            return _mapper.Map<TicketTypeResponseDto>(ticketType);  
        }

        public async Task<IEnumerable<TicketTypeDetailedResponseDto>> GetTicketTypesByOccurenceIdAsync(int occurenceId)
        {
            if (occurenceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(occurenceId), occurenceId,
                    "Event Occurence id must be a positive integer.");

            var ticketTypes = await _repo.GetAllByOccurenceIdAsync(occurenceId);

            return _mapper.Map<IEnumerable<TicketTypeDetailedResponseDto>>(ticketTypes);

        }

        public async Task<IEnumerable<TicketTypeResponseDto>> GetTicketTypesByOccurenceIdForUsersAsync(int occurenceId)
        {
            if (occurenceId <= 0)
                throw new ArgumentOutOfRangeException(nameof(occurenceId), occurenceId,
                    "Event Occurence id must be a positive integer.");

            var ticketTypes = await _repo.GetAllByOccurenceIdAsync(occurenceId);

            return _mapper.Map<IEnumerable<TicketTypeResponseDto>>(ticketTypes);
        }


        public async Task<bool> UpdateAsync(int id, UpdateTicketTypeDto updateTicketType)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Ticket Type id must be a positive integer.");

            if (updateTicketType is null)
                throw new ArgumentNullException(nameof(updateTicketType), "Ticket Type update payload cannot be null.");

            var ticketType = await _repo.GetByIdAsync(id);

            if (ticketType is null)
                throw new KeyNotFoundException($"Ticket Type with ID {id} does not exist.");

            ticketType.Description = updateTicketType.Description;
            ticketType.Price = updateTicketType.Price;

          
            // if quantity changed
            if (updateTicketType.Quantity != ticketType.Quantity)
            {
                var bookedQty = ticketType.Quantity - ticketType.AvailableQuantity;

                if (updateTicketType.Quantity < bookedQty)
                {
                    throw new BusinessException
                        ($"Cannot reduce quantity to { updateTicketType.Quantity } because { bookedQty } tickets are already booked.");
                }

                ticketType.Quantity = updateTicketType.Quantity;
                // new avaliable Qty = updateQty - bookedQuantity
                ticketType.AvailableQuantity = ticketType.Quantity - bookedQty;

            }

            return await _repo.UpdateAsync(ticketType);

        }
    }
}
