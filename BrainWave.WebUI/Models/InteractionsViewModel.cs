using System.ComponentModel.DataAnnotations;

namespace BrainWave.WebUI.Models;

public class InteractionsViewModel
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public bool Status { get; set; }

}