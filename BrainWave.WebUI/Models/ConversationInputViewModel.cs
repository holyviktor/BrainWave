using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class ConversationInputViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Photo is required.")]
        [StringLength(50)]
        public string Photo { get; set; }
        [MinLength(1)]
        [Required(ErrorMessage = "Participants are required.")]
        public List<int> Participants { get; set; }

    }
}
