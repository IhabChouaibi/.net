namespace LivraisonFrontend.ViewModels;

public class SuiviColisPageViewModel
{
    public string Title { get; set; } = "Suivi de colis";
    public string Subtitle { get; set; } = "Consultez l'état de vos livraisons";
    public SuiviColisFilterViewModel Filters { get; set; } = new();
    public IReadOnlyList<SuiviColisItemViewModel> Items { get; set; } = Array.Empty<SuiviColisItemViewModel>();
}
