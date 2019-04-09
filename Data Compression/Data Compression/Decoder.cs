using System.Collections.Generic;
using System.Text;

namespace Data_Compression
{
    public static class Decoder
    {
        public static string GetDecodedMessage(string codedMessage, IReadOnlyDictionary<char, string> codeWords)
        {
            var words = new Dictionary<string, char>(codeWords.Count);

            foreach (var symbol in codeWords.Keys)
            {
                words.Add(codeWords[symbol], symbol);
            }

            return GetDecodedMessage(codedMessage, words);
        }

        public static string GetDecodedMessage(string codedMessage, IReadOnlyDictionary<string, char> codeWords)
        {
            var word = string.Empty;
            var decodedMessage = new StringBuilder();

            foreach (char symbol in codedMessage)
            {
                word += symbol;
                if (codeWords.TryGetValue(word, out char decoded))
                {
                    decodedMessage.Append(decoded);
                    word = string.Empty;
                }
            }

            return decodedMessage.ToString();
        }
    }
}