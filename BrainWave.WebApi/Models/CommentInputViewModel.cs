using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models
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
