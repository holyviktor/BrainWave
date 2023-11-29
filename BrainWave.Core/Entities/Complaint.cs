using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class Complaint
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int ReasonId { get; set; }
        public int ArticleComplaintId { get; set; }
        public User User { get; set; }
        public ReasonComplaint Reason { get; set; }
        public ArticleComplaint ArticleComplaint { get; set; }

    }
}
