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

        // GET: api/Cards/search?keyword=... (Query parameter binding [FromQuery])
        [HttpGet("search")]
        public IActionResult SearchCards([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Search keyword cannot be empty.");

            var results = _cardService.SearchCards(keyword);
            return Ok(results);
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

        // Specialized Update Endpoint: [FromRoute] + [FromBody] DTO
        // PUT: api/Cards/1/update-title
        [HttpPut("{id:int:min(1)}/update-title")]
        public IActionResult UpdateCardTitle([FromRoute] int id, [FromBody] UpdateCardTitleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("New title cannot be empty.");

            var existingCard = _cardService.GetCardById(id);
            if (existingCard == null)
                return NotFound($"Card with ID {id} was not found.");

            _cardService.UpdateCardTitle(id, dto.Title);
            return Ok(new
            {
                Message = "Card title updated successfully.",
                CardId = id,
                UpdatedTitle = dto.Title
            });
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
