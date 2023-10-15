using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models;

public class InteractionsViewModel
{
    [Required]
    public bool Status { get; set; }
}