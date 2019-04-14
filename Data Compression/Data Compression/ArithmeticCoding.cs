using System;
using System.Collections.Generic;
using System.Linq;

namespace Data_Compression
{
    public class ArithmeticCoding : ITextEncodingAlgorithm
    {
        private Dictionary<char, double> slovar;
        private double left, rignt;
        private string rez;
        public IOrderedEnumerable<KeyValuePair<char, double>> sorteslovar;
        private int len;
        public ArithmeticCoding()
        {
            slovar = new Dictionary<char, double>();
            left = 0;
            rignt = 1;
            rez = null;
        }        

        private void CreateSlovar(string str)
        {
            foreach (char ch in str)
            {
                if (slovar.ContainsKey(ch))
                    slovar[ch] += (double)1 / str.Length;
                else
                    slovar.Add(ch, (double)1 / str.Length);
            }
            len = str.Length;
            sorteslovar = slovar.OrderByDescending(s => s.Value);
        }

        private void Step(char ch)
        {
            int i = 0;
            double delt = rignt - left;
            while (sorteslovar.ElementAt(i).Key != ch)
            {
                left += delt * sorteslovar.ElementAt(i).Value;
                i++;
            }
            rignt = left + delt * sorteslovar.ElementAt(i).Value;
        }
        private void DoubleToRez()
        {
            string l = Convert.ToString(left);
            string r = Convert.ToString(rignt);
            if (l.Length > 2 && r.Length > 2)
            {
                int i = 2;
                while (l.Length > i && r.Length > i && l[i] == r[i])
                {
                    rez += l[i];
                    i++;
                }
                l = l.Remove(2, i - 2);
                r = r.Remove(2, i - 2);
                left = Convert.ToDouble(l);
                rignt = Convert.ToDouble(r);
            }
        }
        private void DoubleToRez2()
        {
            string l = Convert.ToString(left);
            string r = Convert.ToString(rignt);

            if (l.Length > 2 && r.Length > 2)
            {
                int i = 2;
                while (l.Length > i && r.Length > i && l[i] == r[i] && rez[i - 2] == l[i])
                {
                    i++;
                }
                l = l.Remove(2, i - 2);
                r = r.Remove(2, i - 2);
                rez = rez.Remove(0, i - 2);
                left = Convert.ToDouble(l);
                rignt = Convert.ToDouble(r);

            }

        }
        private void Final()
        {
            double mid = Math.Round(left + (rignt - left) / 2, 2);
            var m = Convert.ToString(mid).Remove(0, 2);
            rez += m;
        }

        public string Encode(string sourceText)
        {
            CreateSlovar(sourceText);
            foreach (char ch in sourceText)
            {
                Step(ch);
                DoubleToRez();
            }
            Final();
            return rez;
        }

        public string Decode(string codedText)
        {
            string st = null;
            rez = codedText;
            var d = Convert.ToDouble("0," + rez);
            left = 0;
            rignt = 1;
            for (int i = 0; i < len; i++)
            {
                var dl = left;
                var dr = rignt;
                Step(sorteslovar.ElementAt(0).Key);
                int j = 0;
                while (d >= rignt || d <= left)
                {
                    left = dl;
                    rignt = dr;
                    j++;
                    Step(sorteslovar.ElementAt(j).Key);
                }
                st += sorteslovar.ElementAt(j).Key;
                DoubleToRez2();
                d = Convert.ToDouble("0," + rez);
            }
            return st;
        }
    }
}