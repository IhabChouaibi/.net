using LivraisonFrontend.Models;
using LivraisonFrontend.ViewModels;

namespace LivraisonFrontend.Interfaces;

public interface IAuthApiService
{
    Task<AuthResponseViewModel> LoginAsync(LoginRequest request);

    Task<AuthResponseViewModel> RegisterAsync(RegisterRequest request);

    Task<PagedResult<AccountModel>> GetAccountsAsync(SearchFilterViewModel filters);

    Task<AccountModel?> GetAccountByIdAsync(int id);

    Task UpdateAccountAsync(int id, AccountModel model);

    Task DeleteAccountAsync(int id);
}
