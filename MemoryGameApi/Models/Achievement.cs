using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MemoryGameApi.Models
{
    [Table("Achievements")]
    public class Achievement
    {
        [Key]
        public int AchievementId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string BadgeIcon { get; set; } = "🏆";

        [Range(10, 5000)]
        public int BonusPoints { get; set; } = 100;

        // Many-to-Many Relationship (via Linking Table PlayerAchievement)
        [JsonIgnore]
        public ICollection<PlayerAchievement> PlayerAchievements { get; set; } = new List<PlayerAchievement>();
    }
}

