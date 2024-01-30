using System.Security.Cryptography;
using System.Text;

namespace Zusammen;

public class PasswordHasher
{
    public static string salt = "1940GzF+GNOBbg+fjRkWjA==";
    public static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
    
    static string GenerateSalt()
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        
        return Convert.ToBase64String(saltBytes);
    }
}