using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class FilterViewModel
    {
        public List<string> Sort { get; set; } = new List<string> { "Date", "Title", "Popularity", "Price" };
        public List<string> SortOrder { get; set; } = new List<string> { "Asc", "Desc" };
        public List<Category> Categories { get; set; }
    }
}
