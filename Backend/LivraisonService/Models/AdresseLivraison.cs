using System.ComponentModel.DataAnnotations;

namespace LivraisonService.Models;

public class AdresseLivraison
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Adresse { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CodePostal { get; set; } = string.Empty;

    [Required]
    public int ColisId { get; set; }
}
