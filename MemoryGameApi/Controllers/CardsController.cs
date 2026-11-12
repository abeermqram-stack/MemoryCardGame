using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace MemoryGameApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public IActionResult GetAllCards()
        {
            return Ok(_cardService.GetAllCards());
        }

        // Route Constraint :int:range(1,2) and [FromRoute]
        [HttpGet("level/{level:int:range(1,2)}")]
        public IActionResult GetCardsByLevel([FromRoute] int level)
        {
            return Ok(_cardService.GetCardsByLevel(level));
        }

        // Route Constraint :int:min(1) and [FromRoute]
        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetCardById([FromRoute] int id)
        {
            var card = _cardService.GetCardById(id);
            if (card == null)
                return NotFound("No Card was found");

            return Ok(card);
        }

        // Add Operation
        [HttpPost]
        public IActionResult AddCard([FromBody] CardItem card)
        {
            var created = _cardService.AddCard(card);
            return Ok(created);
        }

        // Update Operation
        [HttpPut("{id:int:min(1)}")]
        public IActionResult UpdateCard([FromRoute] int id, [FromBody] CardItem card)
        {
            card.CardId = id;
            var updated = _cardService.UpdateCard(card);
            if (!updated)
                return NotFound("No Card was found");

            return Ok(card);
        }

        // Delete Operation
        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteCard([FromRoute] int id)
        {
            var deleted = _cardService.DeleteCard(id);
            if (!deleted)
                return NotFound("No Card was found");

            return Ok(new { message = "Card Deleted Successfully" });
        }
    }
}
