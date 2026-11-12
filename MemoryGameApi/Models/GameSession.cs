using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MemoryGameApi.Models
{
    [Table("GameSessions")]
    public class GameSession
    {
        [Key]
        public int SessionId { get; set; }

        // Level 1: 6 Pairs (12 cards), Level 2: 10 Pairs (20 cards)
        [Required]
        [Range(1, 2, ErrorMessage = "Level must be 1 or 2")]
        public int Level { get; set; } = 1;

        [Required]
        [Range(6, 10)]
        public int PairsCount { get; set; } = 6;

        [Required]
        [Range(1, 1000)]
        public int Attempts { get; set; }

        [Required]
        [Range(1, 7200)]
        public int TimeElapsedSeconds { get; set; }

        [Required]
        [Range(0, 100000)]
        public int Score { get; set; }

        public bool IsCompleted { get; set; } = true;

        public DateTime PlayedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key for One-to-Many relationship with Player
        [Required]
        public int PlayerId { get; set; }

        // Reference Navigation Property
        public Player? Player { get; set; }
    }
}

