using System.Security.Cryptography;

namespace FileIntegrity.Services
{
    public class HashService
    {
        public string ComputeHash(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("file nor found");
            }

            using var sha256 = SHA256.Create();

            using var stream = File.OpenRead(filePath);

            var hashBytes = sha256.ComputeHash(stream);
            
            return Convert.ToHexString(hashBytes);

            throw new NotImplementedException();
        }
    }
}
