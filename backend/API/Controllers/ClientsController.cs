using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IBarberRepository _barberRepository;
        public ClientsController(IBarberRepository barberRepository)
        {
            _barberRepository = barberRepository;
        }

        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            try { 
            if (client == null)
            {
                return BadRequest();
            }

            var clientString = JsonConvert.SerializeObject(client);
            //var rabbitMqConnection = new RabbitMqConnection();
            //rabbitMqConnection.Connect();
            //rabbitMqConnection.SendMessage("AddClient", clientString);
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
