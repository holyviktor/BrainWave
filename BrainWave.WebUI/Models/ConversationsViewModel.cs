namespace BrainWave.WebUI.Models
{
    public class ConversationsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public MessageViewModel? Message { get; set; }
    }
}
