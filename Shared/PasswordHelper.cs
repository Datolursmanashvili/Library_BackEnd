using System.Text;

namespace Shared
{
    public class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
        public static bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            string enteredPasswordHashed = HashPassword(enteredPassword);
            return string.Equals(enteredPasswordHashed, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
