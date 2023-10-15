using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class ProfileInputViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(30)]
        public string Tag { get; set; }
        [Required]
        public string Photo { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [MinLength(8)]
        [StringLength(30)]
        public string Password { get; set; }

    }
}
