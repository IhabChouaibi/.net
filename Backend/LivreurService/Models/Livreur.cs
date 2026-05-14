using System.ComponentModel.DataAnnotations;

namespace LivreurService.Models;

public class Livreur
{
    public int Id { get; set; }

    [Required]
    [MaxLength(20)]
    public string CIN { get; set; } = string.Empty;

    [Required]
    [MaxLength(120)]
    public string RaisonSocial { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Ville { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string CodePostal { get; set; } = string.Empty;
}
