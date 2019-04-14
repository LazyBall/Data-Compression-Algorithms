using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Data_Compression
{
    /// <summary>
    /// Run-Length Encoding
    /// </summary>
    public class RLE : ITextEncodingAlgorithm
    {
        public string Decode(string codedText)
        {
            var codedTextPattern = @"\A(\((.|\n),\d+\))+\z";
            if (Regex.Match(codedText, codedTextPattern).Success)
            {
                var singleCodePattern = @"(.|\n),\d+";
                var answer = new StringBuilder();

                foreach (Match match in Regex.Matches(codedText, singleCodePattern))
                {
                    answer.Append(match.Value[0], int.Parse(match.Value.Substring(2)));
                }

                return answer.ToString();
            }
            else throw new ArgumentException();
        }

        public string Encode(string sourceText)
        {
            var answer = new StringBuilder();
            int counter = 1;

            for (int i = 1; i < sourceText.Length; i++)
            {
                if (sourceText[i - 1] == sourceText[i]) counter++;
                else
                {
                    answer.Append(string.Format("({0},{1})", sourceText[i - 1], counter));
                    counter = 1;
                }
            }

            answer.Append(string.Format("({0},{1})", sourceText[sourceText.Length - 1], counter));
            return answer.ToString();

        }
    }
}