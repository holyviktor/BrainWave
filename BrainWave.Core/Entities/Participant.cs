namespace BrainWave.Core.Entities
{
    public class Participant
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
        public User User { get; set; }
        public Conversation Conversation { get; set; }

    }
}
