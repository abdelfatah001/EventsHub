using Application.Halls.Dto.Request;
using Application.Halls.Dto.Response;
using Domain.Entities;

namespace Application.Halls
{
    public interface IHallService
    {
        Task<HallResponseDto?> AddAsync(AddHallDto Hall);
        Task<HallResponseDto?> GetByIdAsync(int hallId);
        Task<IEnumerable<HallResponseDto>> GetAllAsync();
        Task<bool> UpdateAsync(int hallId, UpdateHallDto Hall);
        Task<bool> ActivateHallAsync(int hallId);
        Task<bool> DeactivateHallAsync(int hallId);

    }
}
