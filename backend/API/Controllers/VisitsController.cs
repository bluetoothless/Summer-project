using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;
        private readonly IBarberRepository _barberRepository;
        public VisitsController(IVisitService visitService, IBarberRepository barberRepository)
        {
            _visitService = visitService;
            _barberRepository = barberRepository ??
                throw new ArgumentNullException(nameof(barberRepository));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitsController>>> GetVisits()
        {
            try { 
            var visits = await _barberRepository.GetVisitsAsync();
            if (visits == null)
            {
                return NotFound();
            }
            return Ok(visits);
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }

        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> AddVisit([FromBody] Visit visit)
        {
            try { 
            if (visit == null)
            {
                return BadRequest();
            }

            var visitString = JsonConvert.SerializeObject(visit);
            var rabbitMqConnection = new RabbitMqConnection();
            rabbitMqConnection.Connect();
            rabbitMqConnection.SendMessage("AddVisit", visitString);
            //var result = await rabbitMqConnection.receiveMessage();

            return Ok();
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }
    }
}
