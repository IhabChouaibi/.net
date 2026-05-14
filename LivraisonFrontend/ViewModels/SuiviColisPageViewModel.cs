namespace LivraisonFrontend.ViewModels;

public class SuiviColisPageViewModel
{
    public string Title { get; set; } = "Suivi de colis";
    public string Subtitle { get; set; } = "Consultez l'état de vos livraisons";
    public string EmptyMessage { get; set; } = "Aucun colis disponible pour le moment";
    public string EmptyDescription { get; set; } = "Dès qu'un colis vous sera attribué, il apparaîtra ici.";
    public SuiviColisFilterViewModel Filters { get; set; } = new();
    public IReadOnlyList<SuiviColisItemViewModel> Items { get; set; } = Array.Empty<SuiviColisItemViewModel>();
}
