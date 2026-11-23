using MemoryGameApi.Models;
using MemoryGameApi.Services;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("level/{level:int:range(1,2)}")]
        public IActionResult GetCardsByLevel([FromRoute] int level)
        {
            return Ok(_cardService.GetCardsByLevel(level));
        }

        [HttpGet("search")]
        public IActionResult SearchCards([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Search keyword cannot be empty.");

            var results = _cardService.SearchCards(keyword);
            return Ok(results);
        }

        [HttpGet("{id:int:min(1)}")]
        public IActionResult GetCardById([FromRoute] int id)
        {
            var card = _cardService.GetCardById(id);
            if (card == null)
                return NotFound("No Card was found");

            return Ok(card);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCard([FromBody] CardItem card)
        {
            var created = _cardService.AddCard(card);
            return Ok(created);
        }

        [HttpPut("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCard([FromRoute] int id, [FromBody] CardItem card)
        {
            card.CardId = id;
            var updated = _cardService.UpdateCard(card);
            if (!updated)
                return NotFound("No Card was found");

            return Ok(card);
        }

        [HttpPut("{id:int:min(1)}/update-title")]
        [Authorize(Roles = "Admin")]
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

        [HttpDelete("{id:int:min(1)}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCard([FromRoute] int id)
        {
            var deleted = _cardService.DeleteCard(id);
            if (!deleted)
                return NotFound("No Card was found");

            return Ok(new { message = "Card Deleted Successfully" });
        }
    }
}
