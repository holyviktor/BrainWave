using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class ArticleInputViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Title must have min length of 2 and max length of 150.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Price must be between 0 and 2147483647.")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Text is required.")]
        [StringLength(5000, MinimumLength = 5, ErrorMessage = "Text must have min length of 5 and max length of 5000.")]
        public string Text { get; set; }
        
    }
}
