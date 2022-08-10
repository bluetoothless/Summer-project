using API.Entities;
using API.Models;
using API.Services.Abstract;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class BarberRepository : IBarberRepository
    {
        private readonly AppDbContext _context;
        public BarberRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Barber>> GetBarbersAsync()
        {
            return await _context.Barbers.OrderBy(c => c.name).ToListAsync();
        }

        public async Task<Barber> GetBarberByIdAsync(int barberId)
        { 
            var result = await _context.Barbers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.id == barberId);
            return result;
        }

        public async Task<IEnumerable<BarberingService>> GetBarberingServicesAsync()
        {
            return await _context.BarberingServices.OrderBy(c => c.name).ToListAsync();
        }

        public async Task<IEnumerable<Visit>> GetVisitsAsync()
        {
            return await _context.Visits.OrderBy(c => c.date).ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await _context.Clients.OrderBy(c => c.name).ToListAsync();
        }

    }
}
