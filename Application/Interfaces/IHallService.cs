
using Application.DTOs.Hall.Request;
using Application.DTOs.Hall.Response;
using Domain.Entities;

namespace Application.Interfaces
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
