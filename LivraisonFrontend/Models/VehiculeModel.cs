using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.Models;

public class VehiculeModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Couleur")]
    public string Couleur { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Marque")]
    public string Marque { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Matricule")]
    public string Matricule { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Type")]
    public string Type { get; set; } = string.Empty;

    [Display(Name = "Vitesse limite")]
    public int VitesseLimite { get; set; }

    [Display(Name = "Capacité")]
    public int Capacite { get; set; }

    [Display(Name = "Nombre d'essieux")]
    public int NbrEssieux { get; set; }

    [Display(Name = "Nombre de places")]
    public int NbrPlaces { get; set; }
}
