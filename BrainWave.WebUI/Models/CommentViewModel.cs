namespace BrainWave.WebUI.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public UserViewModel User { get; set; }
    }
}
