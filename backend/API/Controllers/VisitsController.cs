using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using System.Net;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitsController : ControllerBase
    {
        private readonly IBarberRepository _barberRepository; 
        public IHubContext<ConfirmationMessageHub> _hubContext;
        public VisitsController(IBarberRepository barberRepository, IHubContext<ConfirmationMessageHub> hubContext)
        {
            _barberRepository = barberRepository ??
                throw new ArgumentNullException(nameof(barberRepository));
            _hubContext = hubContext;
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
                var rabbitMqConnection = new RabbitMqConnection(_hubContext);
                rabbitMqConnection.Connect();
                rabbitMqConnection.SendMessage("AddVisit", visitString);
                rabbitMqConnection.ReceiveMessage();
                
                return Ok();
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.Timeout)
            {
                return NotFound();
            }
        }
    }
}
