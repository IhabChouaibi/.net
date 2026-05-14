using System.Security.Cryptography;
using System.Text;

namespace AuthService.Helpers;

public static class PasswordHasherHelper
{
    public static string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    public static bool Verify(string password, string hashedPassword) =>
        string.Equals(HashPassword(password), hashedPassword, StringComparison.OrdinalIgnoreCase);
}
