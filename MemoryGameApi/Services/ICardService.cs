using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public interface ICardService
    {
        List<CardItem> GetAllCards();
        List<CardItem> GetCardsByLevel(int level);
        CardItem? GetCardById(int id);
        CardItem AddCard(CardItem card);
        bool UpdateCard(CardItem updatedCard);
        bool DeleteCard(int id);
    }
}

