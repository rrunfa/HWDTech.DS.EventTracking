using System.Security.Cryptography;
using System.Text;
using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public class Tracked : ITracked
    {
        private string _hash;

        public static Tracked WithString(string toHash)
        {
            return new Tracked { _hash = CalculateMd5Hash(toHash) };
        }

        public static Tracked WithHash(string hash)
        {
            return new Tracked { _hash = hash };
        }

        public string Hash()
        {
            return _hash;
        }

        private static string CalculateMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
