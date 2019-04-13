using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Compression
{
    public class BurrowsWheelerTransform : ICodingAlgorithm
    {
        public string Decode(string codedText)
        {
            throw new NotImplementedException();
        }

        public string Encode(string sourceText)
        {
            var strBuilder = new StringBuilder(sourceText, sourceText.Length + 32);
            var matrix = new string[sourceText.Length];

            for (int i = 0; i < sourceText.Length; i++)
            {
                matrix[i] = strBuilder.ToString();
                strBuilder.Remove(0, 1);
                strBuilder.Append(sourceText[i]);
            }

            Array.Sort(matrix);
            strBuilder.Clear();
            int pos = -1;

            for(int i = 0; i < sourceText.Length; i++)
            {                
                strBuilder.Append(matrix[i][sourceText.Length - 1]);
                if (pos == -1)
                {
                    if(matrix[i].CompareTo(sourceText)==0)
                    {
                        pos = i;
                    }
                }
            }

            strBuilder.Append(",");
            strBuilder.Append(pos);
            return strBuilder.ToString();
        }
    }
}