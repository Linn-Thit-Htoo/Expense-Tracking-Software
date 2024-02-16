using System.Security.Cryptography;
using System.Text;

namespace ExpenseTrackerApi.Services
{
    public class EncryptService
    {
        private readonly IConfiguration _config;

        public EncryptService(IConfiguration config)
        {
            _config = config;
        }

        #region Encryt Service
        public string EncryptString(string raw, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new((Stream)cryptoStream))
                        {
                            streamWriter.Write(raw);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        #endregion
    }
}
