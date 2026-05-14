using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ColisService.Helpers;

namespace ColisService.Models;

public class Colis
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Libelle { get; set; } = string.Empty;

    public DateTime DateLivraison { get; set; } = DateTime.UtcNow;

    [Required]
    public decimal Montant { get; set; }

    [Required]
    public double Poids { get; set; }

    [Required]
    public double Volume { get; set; }

    [Required]
    public int ClientId { get; set; }

    [Required]
    public int LivreurId { get; set; }

    [Required]
    public int StatutLivraisonId { get; set; } = 1;

    [NotMapped]
    public string StatutLivraisonLabel => StatutLivraisonHelper.GetLabel(StatutLivraisonId);
}
