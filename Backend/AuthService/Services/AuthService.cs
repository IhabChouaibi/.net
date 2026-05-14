using AuthService.Helpers;
using AuthService.Interfaces;
using AuthService.Models;

namespace AuthService.Services;

public class AuthService : IAuthService
{
    private readonly ICompteRepository _compteRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(ICompteRepository compteRepository, JwtHelper jwtHelper)
    {
        _compteRepository = compteRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Les mots de passe ne correspondent pas."
            };
        }

        var existingAccount = await _compteRepository.GetByLoginAsync(request.Login);
        if (existingAccount is not null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Ce login existe deja."
            };
        }

        var compte = new Compte
        {
            Nom = request.Nom,
            Prenom = request.Prenom,
            Email = request.Email,
            Telephone = request.Telephone,
            CIN = request.CIN,
            Adresse = request.Adresse,
            Ville = request.Ville,
            CodePostal = request.CodePostal,
            Login = request.Login,
            Password = PasswordHasherHelper.HashPassword(request.Password),
            Role = "User"
        };

        await _compteRepository.AddAsync(compte);
        await _compteRepository.SaveAsync();

        var token = GenerateJwtToken(compte);
        return new AuthResponse
        {
            Success = true,
            Message = "Compte cree avec succes.",
            Token = token,
            Id = compte.Id,
            UserId = compte.Id,
            CompteId = compte.Id,
            Login = compte.Login,
            Role = compte.Role,
            FullName = compte.FullName
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var compte = await ValidateUserAsync(request.Login, request.Password);
        if (compte is null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Login ou mot de passe invalide."
            };
        }

        return new AuthResponse
        {
            Success = true,
            Message = "Connexion reussie.",
            Token = GenerateJwtToken(compte),
            Id = compte.Id,
            UserId = compte.Id,
            CompteId = compte.Id,
            Login = compte.Login,
            Role = compte.Role,
            FullName = compte.FullName
        };
    }

    public async Task<Compte?> ValidateUserAsync(string login, string password)
    {
        var compte = await _compteRepository.GetByLoginAsync(login);
        if (compte is null)
        {
            return null;
        }

        return PasswordHasherHelper.Verify(password, compte.Password) ? compte : null;
    }

    public string GenerateJwtToken(Compte compte) => _jwtHelper.GenerateJwtToken(compte);

    public async Task<PagedResult<Compte>> GetAccountsAsync(SearchRequest request) =>
        await _compteRepository.GetPagedAsync(request.SearchTerm, request.Status, request.Page, request.PageSize);

    public async Task<Compte?> GetAccountByIdAsync(int id) => await _compteRepository.GetByIdAsync(id);

    public async Task<bool> UpdateAccountAsync(int id, Compte compte)
    {
        var existingAccount = await _compteRepository.GetByIdAsync(id);
        if (existingAccount is null)
        {
            return false;
        }

        existingAccount.Login = compte.Login;
        existingAccount.Role = compte.Role;
        existingAccount.Nom = compte.Nom;
        existingAccount.Prenom = compte.Prenom;
        existingAccount.Email = compte.Email;
        existingAccount.Telephone = compte.Telephone;
        existingAccount.CIN = compte.CIN;
        existingAccount.Adresse = compte.Adresse;
        existingAccount.Ville = compte.Ville;
        existingAccount.CodePostal = compte.CodePostal;

        if (!string.IsNullOrWhiteSpace(compte.Password))
        {
            existingAccount.Password = PasswordHasherHelper.HashPassword(compte.Password);
        }

        _compteRepository.Update(existingAccount);
        await _compteRepository.SaveAsync();
        return true;
    }

    public async Task<bool> DeleteAccountAsync(int id)
    {
        var compte = await _compteRepository.GetByIdAsync(id);
        if (compte is null)
        {
            return false;
        }

        _compteRepository.Delete(compte);
        await _compteRepository.SaveAsync();
        return true;
    }
}
