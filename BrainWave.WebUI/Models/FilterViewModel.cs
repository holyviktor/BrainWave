using BrainWave.Core.Entities;

namespace BrainWave.WebUI.Models
{
    public class FilterViewModel
    {
        public List<string> Sort { get; set; }
        public string[]? Filter { get; set; }
        public List<Category>? Categories { get; set; }

    }
}
