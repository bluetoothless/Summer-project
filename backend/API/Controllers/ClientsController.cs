using API.Entities;
using API.Services.Abstract;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly IBarberRepository _barberRepository;
        public ClientsController(IClientService clientService, IBarberRepository barberRepository)
        {
            _clientService = clientService;
            _barberRepository = barberRepository;
        }

        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] Client client)
        {
            if (client == null)
            {
                return BadRequest();
            }
            /*
            Client client = new Client
            {
                name = clientReceived.name
            };*/

            var clientString = JsonConvert.SerializeObject(client);
            var rabbitMqConnection = new RabbitMqConnection();
            rabbitMqConnection.Connect();
            rabbitMqConnection.SendMessage("AddClient", clientString);
            //var result = await rabbitMqConnection.receiveMessage();
            

            return Ok();
        }

        /*[HttpPost]
        public async Task<IActionResult> AddReview([FromBody] AddReviewWebModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _operationFactory.Create<AddReviewCommand>(q =>
            {
                q.AddReviewModel = model.ToLogicModel();
                q.Token = model.Token;
            }).HandleAsync();

            if (!result.HasSucceeded)
            {
                return BadRequest();
            }

            return Ok(result.Data.ToWebModel());
        }

        [HttpPost("{reviewId}/replies")]
        public async Task<IActionResult> AddReviewReply([FromBody] AddReviewReplyWebModel model, Guid reviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _operationFactory.Create<AddReviewReplyCommand>(q =>
            {
                q.ReviewReplyContent = model.ReplyText;
                q.ReplyToken = model.ReplyToken;
                q.ReviewId = reviewId;
            }).HandleAsync();

            if (!result.HasSucceeded)
            {
                return BadRequest();
            }

            return Ok();
        }*/
    }
}
