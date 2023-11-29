namespace BrainWave.WebUI.Models
{
    public class ComplaintsStatusViewModel
    {
        public int ArticleComplaintId { get; set; }
        public int StatusId { get; set; }
        public List<ArticleComplaintsViewModel> ArticleComplaints { get; set; }
    }
}
