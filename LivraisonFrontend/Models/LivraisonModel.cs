using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.Models;

public class LivraisonModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Adresse")]
    public string Adresse { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ville")]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Code postal")]
    public string CodePostal { get; set; } = string.Empty;

    [Display(Name = "Colis")]
    public int ColisId { get; set; }
}
