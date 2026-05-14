using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.ViewModels;

public class RegisterViewModel
{
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

    [Required]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "La confirmation ne correspond pas au mot de passe.")]
    [Display(Name = "Confirmation")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
