namespace ColisService.Helpers;

public static class StatutLivraisonHelper
{
    public static string GetLabel(int statutId) =>
        statutId switch
        {
            1 => "En attente",
            2 => "En cours",
            3 => "Livre",
            4 => "Annule",
            _ => "Inconnu"
        };
}
