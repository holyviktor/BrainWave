using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class CommentsViewModel
    {
        public List<CommentViewModel>? Comments { get; set; }
        public int CommentsCount { get; set; }
        public bool Status { get; set; }

    }
}
