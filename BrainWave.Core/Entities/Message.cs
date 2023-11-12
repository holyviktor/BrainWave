using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public Conversation Conversation { get; set; }
        public User User { get; set; }
    }
}
