using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MemoryGameApi.Models
{
    [Table("PlayerProfiles")]
    public class PlayerProfile
    {
        [Key]
        public int ProfileId { get; set; }

        [Required]
        [MaxLength(20)]
        public string AvatarEmoji { get; set; } = "🎮";

        [Required]
        [MaxLength(50)]
        public string PreferredTheme { get; set; } = "Neon Space";

        [Range(0, 100000)]
        public int HighestScore { get; set; } = 0;

        [Range(0, 10000)]
        public int TotalGamesPlayed { get; set; } = 0;

        [ForeignKey(nameof(Player))]
        public int PlayerId { get; set; }

        [JsonIgnore]
        public Player? Player { get; set; }
    }
}
