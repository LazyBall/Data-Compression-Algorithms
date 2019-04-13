using System.Collections.Generic;
using System.Text;

namespace Data_Compression
{
    static class Coder
    {

        public static string GetCodedMessage(string message, IReadOnlyDictionary<char, string> codes,
            bool addCodes = false)
        {
            var codedMessage = new StringBuilder(GetCodedMessage(message, codes));

            if (addCodes)
            {
                codedMessage.Append('{');

                foreach (var pair in codes)
                {
                    codedMessage.Append(string.Format("[{0}-{1}]", pair.Key, pair.Value));
                }

                codedMessage.Append('}');
            }

            return codedMessage.ToString();
        }

        static string GetCodedMessage(string message, IReadOnlyDictionary<char, string> codes)
        {
            StringBuilder codedMessage = new StringBuilder(message.Length * 3);

            foreach (var symbol in message)
            {
                codedMessage.Append(codes[symbol]);
            }

            return codedMessage.ToString();
        }

    }
}