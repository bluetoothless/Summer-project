using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IBarberRepository _barberRepository;
        public VisitsController(IBarberRepository barberRepository)
        {
            _barberRepository = barberRepository ??
                throw new ArgumentNullException(nameof(barberRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitsController>>> GetVisits()
        {
            var visits = await _barberRepository.GetVisitsAsync();
            if (visits == null)
            {
                return NotFound();
            }
            return Ok(visits);
        }
    }
}
