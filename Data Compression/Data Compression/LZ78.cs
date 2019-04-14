using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Data_Compression
{
    /// <summary>
    /// LZ78
    /// </summary>
    class LZ78 :ITextEncodingAlgorithm
    {
        public string Encode(string sourceText)
        {
            var dictionary = new Dictionary<string, int>();
            var answer = new StringBuilder();
            var buffer = string.Empty;

            foreach (var symbol in sourceText)
            {
                if (dictionary.ContainsKey(buffer + symbol))
                {
                    buffer += symbol;
                }
                else
                {
                    answer.Append(string.Format("({0},{1})", dictionary[buffer], symbol));
                    dictionary.Add(buffer + symbol, dictionary.Count + 1);
                    buffer = string.Empty;
                }
            }

            return answer.ToString();
        }

        public string Decode(string codedText)
        {
            var codedTextPattern = @"\A(\(\d+,(.|\n)\))+\z";
            if (Regex.Match(codedText, codedTextPattern).Success)
            {
                var singleCodePattern = @"\d+,(.|\n)";
                var answer = new StringBuilder();
                var dictionary = new List<string>
                {
                    string.Empty
                };

                foreach (Match match in Regex.Matches(codedText, singleCodePattern))
                {
                    var position = int.Parse(match.Value.Remove(match.Value.Length - 2));
                    var symbol = match.Value[match.Value.Length - 1];
                    string word = string.Format("{0}{1}", dictionary[position], symbol);
                    answer.Append(word);
                    dictionary.Add(word);
                }

                return answer.ToString();
            }
            else throw new ArgumentException();          
        }
    }
}