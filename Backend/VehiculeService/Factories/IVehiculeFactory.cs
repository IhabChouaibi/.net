using VehiculeService.Models;

namespace VehiculeService.Factories;

public interface IVehiculeFactory
{
    Vehicule CreateVehicule(string type);
}
