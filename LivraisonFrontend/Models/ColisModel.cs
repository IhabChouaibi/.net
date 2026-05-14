using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.Models;

public class ColisModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Libellé")]
    public string Libelle { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Date livraison")]
    public DateTime DateLivraison { get; set; } = DateTime.UtcNow;

    [Display(Name = "Montant")]
    public decimal Montant { get; set; }

    [Display(Name = "Poids")]
    public double Poids { get; set; }

    [Display(Name = "Volume")]
    public double Volume { get; set; }

    [Display(Name = "Client")]
    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un client.")]
    public int ClientId { get; set; }

    [Display(Name = "Livreur")]
    [Range(1, int.MaxValue, ErrorMessage = "Veuillez sélectionner un livreur.")]
    public int LivreurId { get; set; }

    [Display(Name = "Statut")]
    public int StatutLivraisonId { get; set; } = 1;

    public string StatutLivraisonLabel { get; set; } = string.Empty;
}
