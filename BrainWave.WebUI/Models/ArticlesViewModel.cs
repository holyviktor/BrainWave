using BrainWave.Core.Entities;
namespace BrainWave.WebUI.Models
{
    public class ArticlesViewModel
    {
        public ICollection<ArticleViewModel>? Articles { get; set; }
        public FilterViewModel Filter { get; set; }
        public FilterInputViewModel FilterInput { get; set; }
        public bool IsUserAuthorised { get; set; }
    }
}
