using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class OrganizerRepo : IOrganizerRepo
    {
        private readonly AppDbContext _context;
        private readonly IUserRepo _userRepo;
        public OrganizerRepo(AppDbContext context, IUserRepo userRepo) 
        {
            _context = context;
            _userRepo = userRepo;
        }

        public async Task<Organizer?> AddAsync(Organizer organizer)
        {
            ArgumentNullException.ThrowIfNull(organizer, nameof(organizer));

            var user = await _userRepo.AddAsync(organizer.User);

            if (user == null) 
                return null;

            organizer.UserId = user.Id;
            _context .Organizers.Add(organizer);
            return await _context.SaveChangesAsync() > 0 ? organizer : null;

        }

        public async Task<ICollection<Organizer>> GetAllAsync()
        {
            return await _context.Organizers
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Organizer?> GetByIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

            return await _context.Organizers
                .AsNoTracking()
                .Include(organizer => organizer.User)
                .FirstOrDefaultAsync(o => o.OrganizerId == id);
        }

        public async Task<Organizer?> GetByUserIdAsync(int id)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));

            return await _context.Organizers
                .AsNoTracking()
                .Include(organizer => organizer.User)
                .FirstOrDefaultAsync(o => o.UserId == id);
        }

        public async Task<User> GetUserByIdAsync(int organizerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(organizerId, nameof(organizerId));

            return await _context.Organizers
                .AsNoTracking()
                .Select(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == organizerId);
                
        }

        public async Task<bool> UpdateAsync(Organizer organizer)
        {
            ArgumentNullException.ThrowIfNull(organizer, nameof(organizer));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(organizer.OrganizerId, nameof(organizer.OrganizerId));

            _context.Organizers.Update(organizer);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
