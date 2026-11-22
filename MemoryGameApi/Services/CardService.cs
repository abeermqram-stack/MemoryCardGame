using MemoryGameApi.Data;
using MemoryGameApi.Models;

namespace MemoryGameApi.Services
{
    public class CardService : ICardService
    {
        private readonly MemoryGameDbContext _dbContext;

        public CardService(MemoryGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CardItem> GetAllCards() =>
            _dbContext.CardItems.OrderBy(c => c.CardId).ToList();

        public List<CardItem> GetCardsByLevel(int level)
        {
            int requiredPairs = level == 1 ? 6 : 10;

            var activeCards = _dbContext.CardItems
                .Where(c => c.IsActive && c.LevelDifficulty <= level)
                .OrderBy(c => c.CardId)
                .Take(requiredPairs)
                .ToList();

            if (activeCards.Count < requiredPairs)
            {
                activeCards = _dbContext.CardItems
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.CardId)
                    .Take(requiredPairs)
                    .ToList();
            }

            return activeCards;
        }

        public List<CardItem> SearchCards(string keyword) =>
            _dbContext.CardItems
                .Where(c => c.Title.Contains(keyword) || c.Category.Contains(keyword))
                .OrderBy(c => c.CardId)
                .ToList();

        public CardItem? GetCardById(int id) =>
            _dbContext.CardItems.Find(id);

        public CardItem AddCard(CardItem card)
        {
            card.CardId = 0;
            card.Title = (card.Title ?? string.Empty).Trim();
            card.Icon = (card.Icon ?? string.Empty).Trim();
            card.Category = string.IsNullOrWhiteSpace(card.Category) ? "عام" : card.Category.Trim();
            _dbContext.CardItems.Add(card);
            _dbContext.SaveChanges();
            return card;
        }

        public bool UpdateCard(CardItem updatedCard)
        {
            var existing = _dbContext.CardItems.Find(updatedCard.CardId);
            if (existing == null)
                return false;

            existing.Title = updatedCard.Title;
            existing.Icon = updatedCard.Icon;
            existing.Category = updatedCard.Category;
            existing.AccentColor = updatedCard.AccentColor;
            existing.LevelDifficulty = updatedCard.LevelDifficulty;
            existing.IsActive = updatedCard.IsActive;

            _dbContext.SaveChanges();
            return true;
        }

        public bool UpdateCardTitle(int id, string newTitle)
        {
            var existing = _dbContext.CardItems.Find(id);
            if (existing == null)
                return false;

            existing.Title = newTitle;
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteCard(int id)
        {
            var existing = _dbContext.CardItems.Find(id);
            if (existing == null)
                return false;

            _dbContext.CardItems.Remove(existing);
            _dbContext.SaveChanges();
            return true;
        }
    }
}
