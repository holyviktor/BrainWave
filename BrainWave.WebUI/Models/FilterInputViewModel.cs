using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class FilterInputViewModel
    {
        public string? Search { get; set; }
        public string? Sort { get; set; }
        public string? SortOrder { get; set; }
        public int? Category { get; set; }
    }
}
