using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data_Compression
{
    /// <summary>
    /// Burrows–Wheeler transform
    /// </summary>
    //source https://neerc.ifmo.ru/wiki/index.php?title=Преобразование_Барроуза-Уилера#.D0.90.D0.BB.D0.B3.D0.BE.D1.80.D0.B8.D1.82.D0.BC_.D0.B7.D0.B0_.D0.BB.D0.B8.D0.BD.D0.B5.D0.B9.D0.BD.D0.BE.D0.B5_.D0.B2.D1.80.D0.B5.D0.BC.D1.8F
    public class BWT : ITextEncodingAlgorithm
    {       

        public string Encode(string sourceText)
        {
            var matrix = new Permutation[sourceText.Length];
            matrix[0] = new Permutation(sourceText.Length, true);

            for (int j = 0; j < sourceText.Length; j++)
            {
                matrix[0][j] = sourceText[j];
            }

            for (int i = 1; i < sourceText.Length; i++)
            {
                matrix[i] = new Permutation(sourceText.Length, false);

                for (int j = 0; j < sourceText.Length; j++)
                {
                    matrix[i][j] = sourceText[(i + j) % sourceText.Length];
                }

            }

            Array.Sort(matrix);
            var strBuilder = new StringBuilder(sourceText.Length + 11);
            int index = 0;
            var lastColumn = sourceText.Length - 1;

            for (int i = 0; i < sourceText.Length; i++)
            {
                strBuilder.Append(matrix[i][lastColumn]);
                if (matrix[i].Original) index = i;
            }

            strBuilder.Append(",");
            strBuilder.Append(index);
            return strBuilder.ToString();
        }

        //Перестановка
        private class Permutation: IComparable<Permutation>
        {
            readonly char[] _charArray;
            public bool Original { get; set; }

            public Permutation(int length, bool original)
            {
                this._charArray = new char[length];
                this.Original = original;
            }

            public char this[int index]
            {
                get
                {
                    return _charArray[index];
                }
                set
                {
                    _charArray[index] = value;
                }
            }

            public int CompareTo(Permutation other)
            {
                for (int i = 0; i < this._charArray.Length; i++)
                {
                    var compResult = this[i].CompareTo(other[i]);
                    if (compResult != 0) return compResult;
                }
                return 0;
            }
        }

        //Преобразование Барроуза-Уилера, Викиконспекты, Обратное преобразование, линейный алгоритм
        public string Decode(string codedText)
        {
            //массив для получения номера исходной строки в отсортированной матрице (просто чтобы сделать это быстро)
            //11 нулей, так как int.Max равен 2 миллиардам
            var characters = new char[] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' }; 
            int separatorPosition = codedText.Length; //позиция разделителя (запятой)

            for (int i = characters.Length - 1; i > -1; i--)
            {
                var symbol = codedText[--separatorPosition];
                if (symbol == ',') break;
                else characters[i] = symbol;
            }

            
            int index = int.Parse(new string(characters)); //получили номер строки в отсортированной матрице
            var states = new State[separatorPosition]; //массив состояний для преобразованной строки
            var uniqueCharacters = new Dictionary<char, int>(); //словарь уникальных символов и количество их появлений

            for (int i = 0; i < separatorPosition; i++)
            {
                var symbol = codedText[i];
                if (uniqueCharacters.TryGetValue(symbol, out int value)) uniqueCharacters[symbol]++;
                else uniqueCharacters.Add(symbol, 1);
                states[i] = new State(symbol, value);
            }

            var sortedSymbols = (from x in uniqueCharacters orderby x.Key select x.Key).ToArray();
            int sum = 0;
            foreach(var element in sortedSymbols)
            {
                var value = uniqueCharacters[element];
                uniqueCharacters[element] = sum;
                sum += value;
            }

            /*теперь в словаре для каждого уникального символа хранится количество символов в преобразованной строке,
             * которые лексикографически меньше, чем текущий */

            //начинаем формировать исходную (до преобразования) строку
            characters = new char[separatorPosition];

            while (separatorPosition > 0)
            {
                var l = states[index];
                characters[--separatorPosition] = l.Symbol;
                index = l.Count + uniqueCharacters[l.Symbol];
            }

            return new string(characters);
        }

        private struct State
        {
            public char Symbol { get; private set; } //Символ в текущей позиции
            public int Count { get; private set; } //Количество появлений этого же символа до текущей позиции

            public State(char symbol, int count)
            {
                this.Symbol = symbol;
                this.Count = count;
            }
        }
    }
}