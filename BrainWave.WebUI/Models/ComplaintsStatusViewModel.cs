namespace BrainWave.WebUI.Models
{
    public class ComplaintsStatusViewModel
    {
        public int ComplaintId { get; set; }
        public int StatusId { get; set; }
        public List<ComplaintViewModel> Complaints { get; set; }
    }
}
