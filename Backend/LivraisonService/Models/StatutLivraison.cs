using System.ComponentModel.DataAnnotations;

namespace LivraisonService.Models;

public class StatutLivraison
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Libelle { get; set; } = string.Empty;
}
