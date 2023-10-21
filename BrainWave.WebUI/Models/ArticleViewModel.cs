using BrainWave.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public int LikesCount { get; set; }
        public bool IsLiked { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public int SavingsCount { get; set; }
        public bool IsSaved { get; set; }
    }
}
