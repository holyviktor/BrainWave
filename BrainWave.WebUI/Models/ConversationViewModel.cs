namespace BrainWave.WebUI.Models
{
    public class ConversationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public int ParticipantsCount { get; set; }
        public List<MessageViewModel> Messages { get; set; }
        public UserViewModel User { get; set; }
    }
}
