using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class HallRepo : IHallRepo
    {
        private readonly AppDbContext _context;

        public HallRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Hall?> AddAsync(Hall Hall)
        {
            ArgumentNullException.ThrowIfNull(Hall, nameof(Hall));

            _context.Halls.Add(Hall);

            return await _context.SaveChangesAsync() > 0 ? Hall : null;

        }

        public async Task<IEnumerable<Hall>> GetAllAsync()
        {
            return await _context.Halls
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Hall?> GetByIdAsync(int hallId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            return await _context.Halls
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == hallId);
        }

        public async Task<bool> UpdateAsync(Hall Hall)
        {
            ArgumentNullException.ThrowIfNull(Hall, nameof(Hall));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(Hall.Id);

            _context.Halls.Update(Hall);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsActive (int hallId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hallId);

            return await _context.Halls
                .AsNoTracking()
                .AnyAsync(h => h.Id == hallId && h.IsActive);
        }
    }
}
