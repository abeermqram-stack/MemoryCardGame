using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MemoryGameApi.Models
{
    // Linking Table for Many-to-Many Relationship between Player and Achievement
    [Table("PlayerAchievements")]
    public class PlayerAchievement
    {
        // Composite Primary Key (PlayerId + AchievementId) configured via Fluent API
        public int PlayerId { get; set; }

        [JsonIgnore]
        public Player? Player { get; set; }

        public int AchievementId { get; set; }

        public Achievement? Achievement { get; set; }

        // Extra payload properties in the Linking Table (like 'Order' in BookAuthor lecture example)
        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

        [Range(1, 100)]
        public int DisplayOrder { get; set; } = 1;
    }
}

