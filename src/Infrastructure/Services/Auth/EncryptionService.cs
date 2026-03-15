// Infrastructure/Services/Auth/EncryptionService.cs
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services.Auth
{
    public interface IEncryptionService
    {
        string Encrypt(string textToEncrypt, string password, bool returnOnlyNumbersAndLetters = false);
    }

    public class EncryptionService : IEncryptionService
    {
        public string Encrypt(string textToEncrypt, string password, bool returnOnlyNumbersAndLetters = false)
        {
            try
            {
                string text = EncryptInternal(
                    textToEncrypt,
                    password,
                    "777777",
                    "SHA1",
                    2,
                    "GLORY_BE_TO_GOD!",
                    256
                );

                if (returnOnlyNumbersAndLetters)
                {
                    text = StripNonAlphaNumeric(text);
                }
                return text;
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }

        private string EncryptInternal(
            string plainText,
            string passPhrase,
            string saltValue,
            string hashAlgorithm,
            int passwordIterations,
            string initVector,
            int keySize)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            var passwordDeriveBytes = new PasswordDeriveBytes(
                passPhrase,
                Encoding.ASCII.GetBytes(saltValue),
                hashAlgorithm,
                passwordIterations
            );

            using (var rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Mode = CipherMode.CBC;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(
                        memoryStream,
                        rijndaelManaged.CreateEncryptor(
                            passwordDeriveBytes.GetBytes(keySize / 8),
                            Encoding.ASCII.GetBytes(initVector)
                        ),
                        CryptoStreamMode.Write
                    ))
                    {
                        cryptoStream.Write(bytes, 0, bytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        private string StripNonAlphaNumeric(string text)
        {
            return new string(text.Where(c => char.IsLetterOrDigit(c)).ToArray());
        }
    }
}