using WebAPITest.Models;
using WebAPITest.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPITest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly ICardService _card;
        private readonly IClientService _client;

        public CardController(ILogger<CardController> logger, ICardService card, IClientService client)
        {
            _logger = logger;
            _card = card;
            _client = client;
        }

        [HttpGet(Name = "GetCards")]
        public Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
            return _card.GetAllCards();
        }

        [HttpGet("{cardId}", Name = "GetCard")]
        public ActionResult<Card> GetCard(int cardId)
        {
            var card = _card.GetOneCard(cardId).Result;
            return card.Value?.Id == 0
                ? NotFound()
                : card;
        }

        [Authorize]
        [HttpGet("client/{client}", Name = "GetCardByClient")]
        public ActionResult<IEnumerable<Card>> GetCardByClient(string client)
        {
            return _card.GetCardByClient(client).Result;
        }

        [HttpPost]
        public async Task<ActionResult<Card>> Post(Card card)
        {
            if(!await _client.ClientExist(card.Client!)) return BadRequest();

            _card.AddCard(card);

            return new CreatedAtRouteResult("GetCard", card);
        }

        [HttpPut("{cardId}")]
        public async Task<IActionResult> Put(int cardId, Card newCard)
        {
            if(await _card.CardExist(cardId)) return BadRequest();
            var card = _card.GetOneCard(cardId).Result.Value!;
            
            if(newCard.NumberCode is null)
                card.NumberCode = newCard.NumberCode;
            if(newCard.ExpirationDate != default(DateTime))
                card.ExpirationDate = newCard.ExpirationDate;
            if(newCard.OwnerName is not null)
                card.OwnerName = newCard.OwnerName;
            if(newCard.Client is not null && await _client.ClientExist(newCard.Client!))
                card.Client = newCard.Client;

            _card.UpdateCard(card);

            return new CreatedAtRouteResult("GetGroup", newCard);
        }

        [HttpDelete("{cardId}")]
        public async Task<ActionResult<Card>> Delete(int cardId)
        {
            if(!await _card.CardExist(cardId)) return NotFound();

            var card = _card.GetOneCard(cardId).Result.Value!;

            _card.DeleteCard(card);

            return card;
        }
    }
}