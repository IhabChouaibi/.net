using System.ComponentModel.DataAnnotations;

namespace VehiculeService.Models;

public class Vehicule
{
    public int Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Couleur { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Marque { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string Matricule { get; set; } = string.Empty;

    public int VitesseLimite { get; set; }

    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;
}
