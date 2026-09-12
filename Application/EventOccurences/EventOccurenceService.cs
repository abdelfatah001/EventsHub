using Application.EventOccurences.Dto.Request;
using Application.EventOccurences.Dto.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.EventOccurences
{
    public class EventOccurenceService : IEventOccurenceService
    {
        private readonly IEventOccurenceRepo _occurenceRepo; 
        private readonly IEventRepo _eventRepo;
        private readonly IHallRepo _hallRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        
        public EventOccurenceService(IEventOccurenceRepo occurenceRepo, IEventRepo eventRepo, IHallRepo repo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _occurenceRepo = occurenceRepo;
            _eventRepo = eventRepo;
            _hallRepo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OccurenceResponseDto?> AddAsync(AddOccurenceDto newOccurence)
        {
            if (newOccurence is null)
                throw new ArgumentNullException(nameof(newOccurence), "New event occurence payload cannot se null");

            var eventExists = await _eventRepo.ExistsAsync(newOccurence.EventId);

            // Is this event is existed 
            if (!eventExists)
                throw new KeyNotFoundException($"Event with ID {newOccurence.EventId} was not found.");

            if (!await _hallRepo.IsActive(newOccurence.HallId))
                throw new BusinessException($"Hall with Id {newOccurence.HallId} is not active");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Lock this hall
                await _hallRepo.LockHallAsync(newOccurence.HallId);

                // Now nobody else can make a scheduling decision
                // for this hall until this transaction finishes.
                var avaliable = await _occurenceRepo.IsHallAvaliableAsync
                (newOccurence.HallId, newOccurence.StartAt, newOccurence.EndAt);

                if (!avaliable)
                    throw new BusinessException($"Hall with Id {newOccurence.HallId} is not avaliable from {newOccurence.StartAt} to {newOccurence.EndAt}");
            
                var occurence = await _occurenceRepo.AddAsync(_mapper.Map<EventOccurence>(newOccurence));

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return _mapper.Map<OccurenceResponseDto>(occurence);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();   
                throw;
            }

        }

        public async Task<IEnumerable<OccurenceResponseDto>> GetAllAsync()
        {
            var occurences = await _occurenceRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<OccurenceResponseDto>>(occurences);
        }

        public async Task<IEnumerable<OccurenceResponseDto>> GetAllByEventIdAsync(int eventId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(eventId);

            // Q : Am I need to check its existence first or it useless

            var occurences = await _occurenceRepo.GetAllByEventIdAsync(eventId);

            return _mapper.Map<IEnumerable<OccurenceResponseDto>>(occurences);
        }

        public async Task<OccurenceResponseDto?> GetByIdAsync(int occurenceId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(occurenceId);

            var occurence = await _occurenceRepo.GetByIdAsync(occurenceId);

            return _mapper.Map<OccurenceResponseDto?>(occurence);

        }

        // testable
        public async Task<bool> UpdateAsync(int id, UpdateOccurenceDto updatedOccurence)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
            ArgumentNullException.ThrowIfNull(updatedOccurence);

            var occurence = await _occurenceRepo.GetByIdAsync(id);

            if (occurence is null)
                throw new KeyNotFoundException($"Event occurence with ID {id} does not exist.");

            // if there any change in hall or time,
            // check its activity and avaliability 
            if (occurence.HallId != updatedOccurence.HallId ||
                occurence.StartDate != updatedOccurence.StartAt ||
                 occurence.EndDate != updatedOccurence.EndAt)
            {
                // if not active throw exception
                if (!await _hallRepo.IsActive(updatedOccurence.HallId))
                    throw new BusinessException($"Hall with Id {updatedOccurence.HallId} is not active");

                // if not avaliable throw exception
                bool isHallAvaliable = await _occurenceRepo.IsHallAvaliableAsync
                    (updatedOccurence.HallId, updatedOccurence.StartAt, updatedOccurence.EndAt);

                if (!isHallAvaliable) // if not avaliable throw an exception
                {
                    throw new BusinessException
                        ($"Hall with Id {updatedOccurence.HallId} is not avaliable from {updatedOccurence.StartAt} to {updatedOccurence.EndAt}");
                }
                else // if avaliable change them
                {
                    occurence.HallId = updatedOccurence.HallId;
                    occurence.StartDate = updatedOccurence.StartAt;
                    occurence.EndDate = updatedOccurence.EndAt;
                }
            }

            occurence.Status = updatedOccurence.Status;

            return await _occurenceRepo.UpdateAsync(occurence);
        }

    }
}
