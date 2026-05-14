using System.ComponentModel.DataAnnotations;

namespace PaiementService.Models;

public class Paiement
{
    public int Id { get; set; }

    [Required]
    public decimal Montant { get; set; }

    [Required]
    public DateTime DatePaiement { get; set; } = DateTime.UtcNow;

    [Required]
    [MaxLength(30)]
    public string ModePaiement { get; set; } = "Especes";

    [Required]
    public int ColisId { get; set; }
}
