using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IHallRepo
    {
        Task<Hall?> AddAsync(Hall Hall);
        Task<Hall?> GetByIdAsync(int hallId);
        Task<IEnumerable<Hall>> GetAllAsync();
        Task<bool> UpdateAsync(Hall Hall);
        Task<bool> IsActive(int hallId);
        Task LockHallAsync(int hallId);


    }
}
