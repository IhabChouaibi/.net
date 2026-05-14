using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.Models;

public class PaiementModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Montant")]
    public decimal Montant { get; set; }

    [Display(Name = "Date paiement")]
    public DateTime DatePaiement { get; set; } = DateTime.UtcNow;

    [Display(Name = "Mode paiement")]
    public string ModePaiement { get; set; } = "Carte";

    [Display(Name = "Colis")]
    public int ColisId { get; set; }
}
