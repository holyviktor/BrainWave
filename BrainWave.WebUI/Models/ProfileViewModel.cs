using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class ProfileViewModel
    {
        public User User { get; set; }
        public List<Article> Articles { get; set; }
    }
}
