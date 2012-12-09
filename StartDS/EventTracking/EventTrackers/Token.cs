using System.Security.Cryptography;
using System.Text;
using HWdTech.DS.v30;
using HWdTech.DS.v30.PropertyObjects;
using StartDS.EventTracking.Interfaces;

namespace StartDS.EventTracking.EventTrackers
{
    public class Token : IToken
    {
        private string _hash;
        private string _type;
        private string _data;

        public static Token WithMessage(IMessage message)
        {
            var data = new Field<string>("text");
            var type = new Field<string>("type");
            var hash = new Field<string>("hash");

            return new Token()
                {
                    _hash = hash[message], 
                    _type = type[message],
                    _data = data[message]
                };
        }

        public IMessage WrapMessage(IMessage message)
        {
            var data = new Field<string>("text");
            var type = new Field<string>("type");
            var hash = new Field<string>("hash");

            data[message] = _data;
            type[message] = _type;
            hash[message] = _hash;

            return message;
        }

        public Token(string data, string type)
        {
            _data = data;
            _type = type;
            _hash = CalculateMd5Hash(_type);
        }

        private Token() { }

        public string Hash()
        {
            return _hash;
        }

        public string Type()
        {
            return _type;
        }

        public string Data()
        {
            return _data;
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
