using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Le login est obligatoire.")]
    [StringLength(50)]
    [Display(Name = "Login")]
    public string Login { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [StringLength(150)]
    [DataType(DataType.Password)]
    [Display(Name = "Mot de passe")]
    public string Password { get; set; } = string.Empty;
}
