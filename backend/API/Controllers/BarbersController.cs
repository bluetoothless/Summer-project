using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
            try { 
            var barbers = await _barberRepository.GetBarbersAsync();
            if (barbers == null)
            {
                return NotFound();
            }
            return Ok(barbers);
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetBarberById(
            int id, bool includePointsOfInterest = false)
        {
            try
            {
                var barber = await _barberRepository.GetBarberByIdAsync(id);
                if (barber == null)
                {
                    return NotFound();
                }

                return Ok(barber);
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }

        [HttpGet("{id}/booking")]
        public async Task<IActionResult> GetBarberingServices(
            int id, bool includePointsOfInterest = false)
        {
            try { 
            var barberingServices = await _barberRepository.GetBarberingServicesAsync();
            if (barberingServices == null)
            {
                return NotFound();
            }

            return Ok(barberingServices);
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }
    }
}
