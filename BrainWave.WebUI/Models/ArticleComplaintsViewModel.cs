namespace BrainWave.WebUI.Models
{
    public class ArticleComplaintsViewModel
    {
        public string ArticleTitle { get; set; }
        public string ArticleId { get; set; }
        public string StatusName { get; set; }
        public List<ComplaintViewModel> Complaints { get; set; }
    }
}
