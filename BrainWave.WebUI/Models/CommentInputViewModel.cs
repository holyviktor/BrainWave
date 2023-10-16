using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class CommentInputViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        [Required]
        [StringLength(150)]  
        public string Comment { get; set; }

    }
}
