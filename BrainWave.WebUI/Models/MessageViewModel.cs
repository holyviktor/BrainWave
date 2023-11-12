namespace BrainWave.WebUI.Models
{
    public class MessageViewModel
    {
        public UserViewModel User { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
