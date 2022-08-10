using API.Entities;

namespace API.Services.Abstract
{
    public interface IBarberRepository
    {
        Task<IEnumerable<Barber>> GetBarbersAsync();
        Task<Barber> GetBarberByIdAsync(int barberId);
        Task<IEnumerable<BarberingService>> GetBarberingServicesAsync();
        Task<IEnumerable<Visit>> GetVisitsAsync();
        Task<IEnumerable<Client>> GetClientsAsync();
    }
}
