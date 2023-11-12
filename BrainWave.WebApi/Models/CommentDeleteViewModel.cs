using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models
{
    public class CommentDeleteViewModel
    {
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public int CommentId { get; set; }
    }
}
