using System.Security.Cryptography;
using System.Text;

namespace LendingApp.Domain.Utils
{
    public static class StringCipher
    {
        //used for test purposes only, should be saved to Azure Key Vault for security
        public static string key = "xHE9DOZ4dKMNI7HOt5vNYmZ8gmO1tEzG7cYdt1uvvQo=";
        public static string iv = "1tRHER8kPZnN+E8QZyDnUg==";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            using var aes = Aes.Create();
            var keybytes = Convert.FromBase64String(key);
            var ivbytes = Convert.FromBase64String(iv);

            using var encryptor = aes.CreateEncryptor(keybytes, ivbytes);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }
            return ToUrlSafeBase64(ms.ToArray());
        }

        /// <summary>
        /// Decrypts the input string using AES-256.
        /// </summary>
        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return string.Empty;

            using var aes = Aes.Create();
            var keybytes = Convert.FromBase64String(key);
            var ivbytes = Convert.FromBase64String(iv);

            using var decryptor = aes.CreateDecryptor(keybytes, ivbytes);
            using var ms = new MemoryStream(FromUrlSafeBase64(cipherText));
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Converts byte array to URL-safe Base64 string
        /// Replaces + with -, / with _, and removes padding =
        /// </summary>
        private static string ToUrlSafeBase64(byte[] data)
        {
            string base64 = Convert.ToBase64String(data);
            return base64
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }

        /// <summary>
        /// Converts URL-safe Base64 string back to byte array
        /// </summary>
        private static byte[] FromUrlSafeBase64(string urlSafeBase64)
        {
            string base64 = urlSafeBase64
                .Replace('-', '+')
                .Replace('_', '/');

            // Add padding back
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Convert.FromBase64String(base64);
        }
    }
}
