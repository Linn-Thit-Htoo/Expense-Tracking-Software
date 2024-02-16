using System.Security.Cryptography;
using System.Text;

namespace ExpenseTrackerApi.Services
{
    public class DecryptService
    {
        #region Decrypt Service
        public string DecryptString(string encrypt_pwd, string key)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encrypt_pwd);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
