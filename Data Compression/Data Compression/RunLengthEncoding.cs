using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Compression
{
    public class RunLengthEncoding : ICodingAlgorithm
    {
        public string Decode(string codedText)
        {
            throw new NotImplementedException();
        }

        public string Encode(string sourceText)
        {
            var strBuilder = new StringBuilder();
            int counter = 1;

            for (int i = 1; i < sourceText.Length; i++)
            {
                if (sourceText[i - 1] == sourceText[i])
                {
                    counter++;
                }
                else
                {
                    strBuilder.Append(string.Format("<{0},{1}>", sourceText[i - 1], counter));
                    counter = 1;
                }
            }

            strBuilder.Append(string.Format("<{0},{1}>", sourceText[sourceText.Length - 1], counter));
            return strBuilder.ToString();

        }
    }
}