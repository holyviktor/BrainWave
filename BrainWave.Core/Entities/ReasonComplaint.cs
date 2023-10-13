using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainWave.Core.Entities
{
    public class ReasonComplaint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Complaint>? Complaints { get; set; }
    }
}
