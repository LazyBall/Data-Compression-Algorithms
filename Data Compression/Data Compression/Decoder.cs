using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System;

namespace Data_Compression
{
    static class Decoder
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

        public static string GetDecodedMessage(string codedMessage, IReadOnlyDictionary<string, char> codeWords = null)
        {
            if (codeWords == null) codeWords = GetCodeWords(codedMessage);
            var word = new StringBuilder(32);
            var decodedMessage = new StringBuilder();

            foreach (char symbol in codedMessage)
            {
                word.Append(symbol);
                if (codeWords.TryGetValue(word.ToString(), out char decoded))
                {
                    decodedMessage.Append(decoded);
                    word.Clear();
                }
            }

            return decodedMessage.ToString();
        }

        static IReadOnlyDictionary<string, char> GetCodeWords(string codedMessage)
        {
            var patternAllCodes = @"\{(\[(.|\n)-[^\]]+\])+\}\z";
            var patternOneCode = @"(.|\n)-[^\]]+";
            var matchAllCodes = Regex.Match(codedMessage, patternAllCodes);
            if (matchAllCodes.Success)
            {
                var dictionary = new Dictionary<string, char>();

                foreach (Match matchCode in Regex.Matches(matchAllCodes.Value, patternOneCode))
                {
                    var code = matchCode.Value.Substring(2);
                    dictionary.Add(code, matchCode.Value[0]);                    
                }

                return dictionary;
            }
            else
            {
                throw new ArgumentException();
            }
        }

    }
}