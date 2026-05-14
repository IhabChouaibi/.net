using System.ComponentModel.DataAnnotations;

namespace ClientService.Models;

public class Client
{
    public int Id { get; set; }

    public int? CompteId { get; set; }

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
}
