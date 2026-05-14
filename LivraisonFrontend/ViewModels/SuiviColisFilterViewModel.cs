using System.ComponentModel.DataAnnotations;

namespace LivraisonFrontend.ViewModels;

public class SuiviColisFilterViewModel
{
    [Display(Name = "Recherche")]
    public string SearchTerm { get; set; } = string.Empty;

    [Display(Name = "Statut")]
    public string Status { get; set; } = string.Empty;

    [Display(Name = "Ville")]
    public string Ville { get; set; } = string.Empty;

    [Display(Name = "Date livraison")]
    [DataType(DataType.Date)]
    public DateTime? DateLivraison { get; set; }
}
