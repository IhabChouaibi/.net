using ClientService.Helpers;
using ClientService.Interfaces;
using ClientService.Models;

namespace ClientService.Services;

public class ClientCrudService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientCrudService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<PagedResult<Client>> GetPagedAsync(SearchRequest request) =>
        await _clientRepository.GetPagedAsync(request.SearchTerm, request.SortBy, request.SortDirection, request.Page, request.PageSize);

    public async Task<IEnumerable<Client>> GetAllAsync() => await _clientRepository.GetAllAsync();

    public async Task<Client?> GetByIdAsync(int id) => await _clientRepository.GetByIdAsync(id);

    public async Task<Client?> GetByCompteIdAsync(int compteId) => await _clientRepository.GetByCompteIdAsync(compteId);

    public async Task<Client> RegisterAsync(Client client)
    {
        if (client.CompteId is null or <= 0)
        {
            throw new ApplicationException("Le CompteId est obligatoire pour lier le client au compte.");
        }

        var existingClient = await _clientRepository.GetByCompteIdAsync(client.CompteId.Value);
        if (existingClient is not null)
        {
            throw new ApplicationException("Un client existe deja pour ce compte.");
        }

        await _clientRepository.AddAsync(client);
        await _clientRepository.SaveAsync();
        return client;
    }

    public async Task<Client> AddAsync(Client client)
    {
        await _clientRepository.AddAsync(client);
        await _clientRepository.SaveAsync();
        return client;
    }

    public async Task<bool> UpdateAsync(int id, Client client)
    {
        var existingClient = await _clientRepository.GetByIdAsync(id);
        if (existingClient is null)
        {
            return false;
        }

        existingClient.Nom = client.Nom;
        existingClient.Prenom = client.Prenom;
        existingClient.Email = client.Email;
        existingClient.Telephone = client.Telephone;
        existingClient.CIN = client.CIN;
        existingClient.Adresse = client.Adresse;
        existingClient.Ville = client.Ville;
        existingClient.CodePostal = client.CodePostal;
        existingClient.CompteId = client.CompteId;

        _clientRepository.Update(existingClient);
        await _clientRepository.SaveAsync();
        return true;
    }

    public async Task<bool> UpdateByCompteIdAsync(int compteId, Client client)
    {
        var existingClient = await _clientRepository.GetByCompteIdAsync(compteId);
        if (existingClient is null)
        {
            return false;
        }

        existingClient.Nom = client.Nom;
        existingClient.Prenom = client.Prenom;
        existingClient.Email = client.Email;
        existingClient.Telephone = client.Telephone;
        existingClient.CIN = client.CIN;
        existingClient.Adresse = client.Adresse;
        existingClient.Ville = client.Ville;
        existingClient.CodePostal = client.CodePostal;

        _clientRepository.Update(existingClient);
        await _clientRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingClient = await _clientRepository.GetByIdAsync(id);
        if (existingClient is null)
        {
            return false;
        }

        _clientRepository.Delete(existingClient);
        await _clientRepository.SaveAsync();
        return true;
    }
}
