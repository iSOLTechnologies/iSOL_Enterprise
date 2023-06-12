using System.Security.Cryptography;

namespace iSOL_Enterprise.Dal.Home
{
    public static class PasswordHelperDal
    {

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        // Hash the password with salt using SHA256
        public static string HashPassword(this string password, byte[] salt)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] saltedPasswordBytes = new byte[passwordBytes.Length + salt.Length];

            Buffer.BlockCopy(passwordBytes, 0, saltedPasswordBytes, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, saltedPasswordBytes, passwordBytes.Length, salt.Length);

            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                byte[] hashedBytes = sha256.ComputeHash(saltedPasswordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }
        
        public static bool VerifyPassword(string password, string hashedPassword, byte[] salt)
        {
            string hashedPasswordAttempt = HashPassword(password, salt);
            return hashedPassword == hashedPasswordAttempt;
        }
    }
}
