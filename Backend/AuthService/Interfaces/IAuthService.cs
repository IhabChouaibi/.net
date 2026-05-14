using AuthService.Helpers;
using AuthService.Models;

namespace AuthService.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<Compte?> ValidateUserAsync(string login, string password);
    string GenerateJwtToken(Compte compte);
    Task<PagedResult<Compte>> GetAccountsAsync(SearchRequest request);
    Task<Compte?> GetAccountByIdAsync(int id);
    Task<bool> UpdateAccountAsync(int id, Compte compte);
    Task<bool> DeleteAccountAsync(int id);
}
