namespace LivraisonFrontend.ViewModels;

public class SuiviColisItemViewModel
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public DateTime DateLivraison { get; set; }
    public decimal Montant { get; set; }
    public double Poids { get; set; }
    public double Volume { get; set; }
    public int ClientId { get; set; }
    public int LivreurId { get; set; }
    public int StatutLivraisonId { get; set; }
    public string Statut { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
}
