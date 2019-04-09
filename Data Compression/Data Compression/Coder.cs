using System.Collections.Generic;
using System.Text;

namespace Data_Compression
{
    public static class Coder
    {
        public static string GetCodedMessage(string message, IReadOnlyDictionary<char, string> codes)
        {
            StringBuilder codedMessage = new StringBuilder();

            foreach (var symbol in message)
            {
                codedMessage.Append(codes[symbol]);
            }

            return codedMessage.ToString();
        }
    }
}