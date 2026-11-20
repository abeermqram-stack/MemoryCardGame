using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface ICardService
    {
        List<CardItem> GetAllCards();
        List<CardItem> GetCardsByLevel(int level);
        List<CardItem> SearchCards(string keyword);
        CardItem? GetCardById(int id);
        CardItem AddCard(CardItem card);
        bool UpdateCard(CardItem updatedCard);
        bool UpdateCardTitle(int id, string newTitle);
        bool DeleteCard(int id);
    }
}

