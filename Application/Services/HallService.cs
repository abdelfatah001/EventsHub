using Application.DTOs.Hall.Request;
using Application.DTOs.Hall.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class HallService : IHallService
    {
        private readonly IHallRepo _hallRepo;
        private readonly IEventOccurenceRepo _occurenceRepo;
        private readonly IMapper _mapper;

        public HallService (IHallRepo hallRepo, IEventOccurenceRepo occurenceRepo, IMapper mapper)
        {
            _hallRepo = hallRepo;
            _occurenceRepo = occurenceRepo;
            _mapper = mapper;
        }
        public async Task<HallResponseDto?> AddAsync(AddHallDto Hall)
        {
            if (Hall is null)
                throw new ArgumentNullException(nameof(Hall), "Hall payload cannot be null.");


            var hall = await _hallRepo.AddAsync(_mapper.Map<Hall>(Hall));

            return _mapper.Map<HallResponseDto?>(hall);
        }


        public async Task<IEnumerable<HallResponseDto>> GetAllAsync()
        {
            var halls = await _hallRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<HallResponseDto>>(halls);
        }

        public async Task<HallResponseDto?> GetByIdAsync(int hallId)
        {
            if (hallId <= 0)
                throw new ArgumentOutOfRangeException(nameof(hallId), hallId, "hall Id must be a positive integer.");

            var hall = await _hallRepo.GetByIdAsync(hallId);

            return _mapper.Map<HallResponseDto>(hall);

        }

        public async Task<bool> UpdateAsync(int id, UpdateHallDto Hall)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), id, "Event category id must be a positive integer.");

            if (Hall is null)
                throw new ArgumentNullException(nameof(Hall), "Hall update payload cannot be null.");

            var hall = await _hallRepo.GetByIdAsync(id);

            if (hall is null)
                throw new KeyNotFoundException($"Hall with ID {id} does not exist.");

            hall .Name = Hall.Name;
            hall.Capacity = Hall.Capacity;

            return await _hallRepo.UpdateAsync(hall);

        }

        public async Task<bool> ActivateHallAsync(int hallId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            if (await _hallRepo.IsActive(hallId))
                return true;

            var hall = await _hallRepo.GetByIdAsync(hallId);

            if (hall is null)
                throw new KeyNotFoundException($"Hall with Id {hallId} is not existed");

            hall.IsActive = true;

            return await _hallRepo.UpdateAsync(hall);
        }

        

        public async Task<bool> DeactivateHallAsync(int hallId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            // if it booked can't be deactivated
            if (await _occurenceRepo.IsHallBooked(hallId))
                throw new BusinessException($"Hall with Id {hallId} is booked in future event,can't be deactivated");

            // if it already not active, return true
            if (!await _hallRepo.IsActive(hallId))
                return true;

            var hall = await _hallRepo.GetByIdAsync(hallId);

            if (hall is null)
                throw new KeyNotFoundException($"Hall with Id {hallId} is not existed");

            hall.IsActive = false;

            return await _hallRepo.UpdateAsync(hall);

        }

    }
}
