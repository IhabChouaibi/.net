namespace DashboardService.Models;

public class ClientItem
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
}

public class LivreurItem
{
    public int Id { get; set; }
    public string CIN { get; set; } = string.Empty;
    public string RaisonSocial { get; set; } = string.Empty;
}

public class ColisItem
{
    public int Id { get; set; }
    public string Libelle { get; set; } = string.Empty;
    public DateTime DateLivraison { get; set; }
    public decimal Montant { get; set; }
    public int ClientId { get; set; }
    public int LivreurId { get; set; }
    public int StatutLivraisonId { get; set; }
    public string StatutLivraisonLabel { get; set; } = string.Empty;
}

public class PaiementItem
{
    public int Id { get; set; }
    public decimal Montant { get; set; }
    public DateTime DatePaiement { get; set; }
    public string ModePaiement { get; set; } = string.Empty;
    public int ColisId { get; set; }
}
