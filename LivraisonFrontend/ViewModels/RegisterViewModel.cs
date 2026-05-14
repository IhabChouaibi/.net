using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(80)]
    [Display(Name = "Nom")]
    public string Nom { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(80)]
    [Display(Name = "Prénom")]
    public string Prenom { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'email est obligatoire.")]
    [EmailAddress(ErrorMessage = "Format d'email invalide.")]
    [StringLength(120)]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le téléphone est obligatoire.")]
    [StringLength(30)]
    [Display(Name = "Téléphone")]
    public string Telephone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le CIN est obligatoire.")]
    [StringLength(20)]
    [Display(Name = "CIN")]
    public string CIN { get; set; } = string.Empty;

    [Required(ErrorMessage = "L'adresse est obligatoire.")]
    [StringLength(180)]
    [Display(Name = "Adresse")]
    public string Adresse { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ville est obligatoire.")]
    [StringLength(80)]
    [Display(Name = "Ville")]
    public string Ville { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le code postal est obligatoire.")]
    [StringLength(10)]
    [Display(Name = "Code postal")]
    public string CodePostal { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le login est obligatoire.")]
    [StringLength(50)]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [StringLength(150, MinimumLength = 6, ErrorMessage = "Le mot de passe doit contenir au moins 6 caractères.")]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "La confirmation est obligatoire.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "La confirmation ne correspond pas au mot de passe.")]
    [Display(Name = "Confirmation")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
