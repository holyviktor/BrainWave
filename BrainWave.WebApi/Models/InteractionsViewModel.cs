using System.ComponentModel.DataAnnotations;

namespace IdentityApi.Models;

public class InteractionsViewModel
{
    [Required]
    public int ArticleId { get; set; }
    [Required]
    public bool Status { get; set; }

}