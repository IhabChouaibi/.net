using System.ComponentModel.DataAnnotations;

namespace AuthService.Models;

public class Compte
{
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Nom { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Prenom { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(120)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Telephone { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CIN { get; set; } = string.Empty;

    [Required]
    [MaxLength(180)]
    public string Adresse { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CodePostal { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Login { get; set; } = string.Empty;

    [MaxLength(250)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = "User";

    public string FullName => $"{Prenom} {Nom}".Trim();
}
