using System.ComponentModel.DataAnnotations;

namespace VehiculeService.Models;

public class VehiculeRequest
{
    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public string Couleur { get; set; } = string.Empty;

    [Required]
    public string Marque { get; set; } = string.Empty;

    [Required]
    public string Matricule { get; set; } = string.Empty;

    public int VitesseLimite { get; set; }
    public int Capacite { get; set; }
    public int NbrEssieux { get; set; }
    public int NbrPlaces { get; set; }
}
