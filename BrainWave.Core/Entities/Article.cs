using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
        public Category Category { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Saving> Savings { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
