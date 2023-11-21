namespace BrainWave.WebUI.Models
{
    public class ComplaintViewModel
    {
        public int Id { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleId { get; set; }
        public string Text { get; set; }
        public string ReasonName { get; set; }
        public string StatusName { get; set; }
    }
}
