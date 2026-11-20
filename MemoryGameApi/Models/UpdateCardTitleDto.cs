using System.ComponentModel.DataAnnotations;

namespace MemoryGameApi.Models
{
    public class UpdateCardTitleDto
    {
        [Required(ErrorMessage = "New card title is required")]
        [MaxLength(80)]
        public string Title { get; set; } = string.Empty;
    }
}

