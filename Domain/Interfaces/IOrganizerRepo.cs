using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IOrganizerRepo
    {
        Task<Organizer?> AddAsync(Organizer organizer);
        Task<ICollection<Organizer>> GetAllAsync();
        Task<Organizer?> GetByIdAsync(int id);
        Task<bool> UpdateAsync (Organizer organizer);
    }
}
