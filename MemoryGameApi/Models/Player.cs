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

        public PlayerProfile? Profile { get; set; }

        public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

        public ICollection<PlayerAchievement> PlayerAchievements { get; set; } = new List<PlayerAchievement>();
    }
}
