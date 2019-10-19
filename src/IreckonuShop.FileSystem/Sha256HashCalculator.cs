using System.Security.Cryptography;
using System.Text;

namespace IreckonuShop.FileSystem
{
    public class Sha256HashCalculator : IHashCalculator
    {
        public string Calculate(string data)
        {
            using (var sha256Hash = SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));
                var builder = new StringBuilder();
                foreach (var item in bytes)
                {
                    builder.Append(item.ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}