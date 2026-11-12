using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoryGameApi.Models
{
    [Table("Players")]
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        [MaxLength(150)]
        [Column("PlayerEmail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(100)]
        public string Password { get; set; } = "123456";

        [Required]
        [MaxLength(30)]
        public string Role { get; set; } = "Player";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // 1. One-to-One Relationship: One Player has One PlayerProfile
        public PlayerProfile? Profile { get; set; }

        // 2. One-to-Many Relationship: One Player has Many GameSessions
        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

        // 3. Many-to-Many Relationship (via Linking Table PlayerAchievement)
        public ICollection<PlayerAchievement> PlayerAchievements { get; set; } = new List<PlayerAchievement>();
    }
}
