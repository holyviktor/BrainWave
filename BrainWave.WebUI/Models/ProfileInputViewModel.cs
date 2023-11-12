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
        [Required(ErrorMessage = "Tag is required.")]
        [StringLength(30, ErrorMessage = "Tag must have max length of 30.")]
        public string Tag { get; set; }
        public string Photo { get; set; }
        [StringLength(500, ErrorMessage = "Description must have max length of 500.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must have min length of 8 and max length of 30.")]
        public string Password { get; set; }

    }
}
