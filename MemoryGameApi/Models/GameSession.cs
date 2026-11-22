using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoryGameApi.Models
{
    [Table("GameSessions")]
    public class GameSession
    {
        [Key]
        public int SessionId { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Level must be 1 or 2")]
        public int Level { get; set; } = 1;

        [Required]
        [Range(1, 100)]
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

        [Required]
        public int PlayerId { get; set; }

        public Player? Player { get; set; }
    }
}
