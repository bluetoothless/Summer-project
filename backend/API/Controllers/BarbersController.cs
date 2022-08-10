using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BarbersController : ControllerBase
    {
        private readonly IBarberRepository _barberRepository;
        public BarbersController(IBarberRepository barberRepository)
        {
            _barberRepository = barberRepository ??
                throw new ArgumentNullException(nameof(barberRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Barber>>> GetBarbers()
        {
            var barbers = await _barberRepository.GetBarbersAsync();
            if (barbers == null)
            {
                return NotFound();
            }
            /*var salon = await _context.Salons
                .Include(s => s.Address)
                .AsNoTracking()
                .FirstAsync(x => x.Id == query.SalonId);*/
            return Ok(barbers);
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBarberById(
            int id, bool includePointsOfInterest = false)
        {
            var barber = await _barberRepository.GetBarberByIdAsync(id);
            if (barber == null)
            {
                return NotFound();
            }

            return Ok(barber);
        }

        [HttpGet("{id}/booking")]
        public async Task<IActionResult> GetBarberingServices(
            int id, bool includePointsOfInterest = false)
        {
            var barberingServices = await _barberRepository.GetBarberingServicesAsync();
            if (barberingServices == null)
            {
                return NotFound();
            }

            return Ok(barberingServices);
        }
    }
}
