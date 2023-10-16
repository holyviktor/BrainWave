using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class CommentDeleteViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public int CommentId { get; set; }
    }
}
