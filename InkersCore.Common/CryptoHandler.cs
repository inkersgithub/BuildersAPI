using System.Security.Cryptography;
using System.Text;

namespace InkersCore.Common
{
    public static class CryptoHandler
    {
        /// <summary>
        /// Function to get Md5Hash of input string
        /// </summary>
        /// <param name="input">Input</param>
        /// <returns>String</returns>
        public static string GetMd5Hash(string input)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
