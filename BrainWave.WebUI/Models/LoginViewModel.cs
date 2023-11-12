using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [MinLength(8)]
        [StringLength(30)]
        public string Password { get; set; }
    }
}
