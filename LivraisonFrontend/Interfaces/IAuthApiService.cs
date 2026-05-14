using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Interfaces;

public interface IAuthApiService
{
    Task<AuthResponse> LoginAsync(LoginRequest request);

    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    Task<PagedResult<AccountModel>> GetAccountsAsync(SearchFilterViewModel filters);

    Task<AccountModel?> GetAccountByIdAsync(int id);

    Task UpdateAccountAsync(int id, AccountModel model);

    Task DeleteAccountAsync(int id);
}
