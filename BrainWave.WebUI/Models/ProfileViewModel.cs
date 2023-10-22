using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public ICollection<ArticleViewModel>? Articles { get; set; }
        public int Followers { get; set; }
        public int Followings { get; set; }
    }
}
