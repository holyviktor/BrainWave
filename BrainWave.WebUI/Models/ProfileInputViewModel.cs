using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class ProfileInputViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        [StringLength(50)]
        public string Surname { get; set; }
        public string? Photo { get; set; }
        [StringLength(500, ErrorMessage = "Description must have max length of 500.")]
        public string Description { get; set; }

    }
}
