using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.Models;

public class LivreurModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "CIN")]
    public string CIN { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Raison sociale")]
    public string RaisonSocial { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ville")]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Code postal")]
    public string CodePostal { get; set; } = string.Empty;
}
