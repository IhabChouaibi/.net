using VehiculeService.Models;

namespace VehiculeService.Factories;

public class VehiculeFactory : IVehiculeFactory
{
    public Vehicule CreateVehicule(string type) =>
        type.ToLower() switch
        {
            "camion" => new Camion { Type = "Camion" },
            "voiture" => new Voiture { Type = "Voiture" },
            _ => throw new ArgumentException("Type de vehicule non supporte.")
        };
}
