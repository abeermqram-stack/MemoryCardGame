using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MemoryGameApi.Models
{
    [Table("PlayerAchievements")]
    public class PlayerAchievement
    {
        public int PlayerId { get; set; }

        [JsonIgnore]
        public Player? Player { get; set; }

        public int AchievementId { get; set; }

        public Achievement? Achievement { get; set; }

        public DateTime UnlockedAt { get; set; } = DateTime.UtcNow;

        [Range(1, 100)]
        public int DisplayOrder { get; set; } = 1;
    }
}
