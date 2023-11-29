using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class ArticleComplaint
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int StatusId { get; set; }
        public StatusComplaint Status { get; set; }
        public Article Article { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
    }
}
