using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.ViewModels;

public class ProfilViewModel
{
    public int? CompteId { get; set; }

    [Required]
    [Display(Name = "Nom")]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Téléphone")]
    public string Telephone { get; set; } = string.Empty;

    [Required]
    [Display(Name = "CIN")]
    public string CIN { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Adresse")]
    public string Adresse { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Ville")]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Code postal")]
    public string CodePostal { get; set; } = string.Empty;

    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;
}
