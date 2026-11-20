using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MemoryGameApi.Models
{
    [Table("CardItems")]
    public class CardItem
    {
        [Key]
        public int CardId { get; set; }

        [Required(ErrorMessage = "Card title is required")]
        [MaxLength(80)]
        [Column("CardTitle")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Card icon or emoji is required")]
        [MaxLength(150)]
        public string Icon { get; set; } = string.Empty;

        [Required]
        [MaxLength(60)]
        public string Category { get; set; } = "General";

        [Required]
        [MaxLength(30)]
        public string AccentColor { get; set; } = "#6366f1";

        [Range(1, 2, ErrorMessage = "Level must be 1 (6 pairs) or 2 (10 pairs)")]
        public int LevelDifficulty { get; set; } = 1;

        public bool IsActive { get; set; } = true;
    }
}
