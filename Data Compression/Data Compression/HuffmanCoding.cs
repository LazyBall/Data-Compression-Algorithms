using System;
using System.Collections.Generic;
using System.Text;

namespace Data_Compression
{
    // source http://neerc.ifmo.ru/wiki/index.php?title=Алгоритм_Хаффмана_для_n_ичной_системы_счисления
    // http://trinary.ru/kb/d0085167-8832-466b-8cba-cc2e377e84c3
    public class HuffmanCoding : ICodingAlgorithm
    {
        public IReadOnlyDictionary<char, string> CodeWords { get; private set; }
        private readonly int _numberSystem;

        public HuffmanCoding(int numberSystem)
        {
            if (numberSystem < 2) throw new ArgumentException();
            this._numberSystem = numberSystem;
        }

        public IReadOnlyDictionary<char, string> CreateCodeWords(IReadOnlyDictionary<char, int> frequencyDictionary)
        {
            var tree = new List<Node>(frequencyDictionary.Count);

            foreach (var key in frequencyDictionary.Keys)
            {
                tree.Add(new Node(key, frequencyDictionary[key], null));
            }

            int m = frequencyDictionary.Count;
            if (m > _numberSystem)
            {

                while ((m - _numberSystem) % (_numberSystem - 1) != 0)
                {
                    m++;
                }

            }
            int start = _numberSystem - (m - frequencyDictionary.Count); //сколько нужно взять на первом шаге

            // первый шаг
            tree.Sort();
            var nodes = new List<Node>(start);
            int sumNodes = 0;
            while (tree.Count > 0 && nodes.Count != start)
            {
                int last = tree.Count - 1;
                nodes.Add(tree[last]);
                sumNodes += tree[last].Value;
                tree.RemoveAt(last);
            }
            //

            tree.Add(new Node('0', sumNodes, nodes));

            while (tree.Count > 1)
            {
                tree.Sort();
                var list = new List<Node>(_numberSystem);
                int sum = 0;

                while (list.Count < _numberSystem)
                {
                    int last = tree.Count - 1;
                    list.Add(tree[last]);
                    sum += tree[last].Value;
                    tree.RemoveAt(last);
                }

                tree.Add(new Node('0', sum, list));
            }

            return GetCodesWhenTraversing(tree[0]);
        }

        public IReadOnlyDictionary<char, string> CreateCodeWords(string message)
        {
            var frequencyDictionary = GetFrequencyDictionary(message);
            return CreateCodeWords(frequencyDictionary);
        }

        public string Encode(string sourceText)
        {
            CodeWords = CreateCodeWords(sourceText);
            return Coder.GetCodedMessage(sourceText, CodeWords);
        }

        public string Decode(string codedText)
        {
            if (CodeWords == null)
            {
                throw new ArgumentException();
            }
            return Decoder.GetDecodedMessage(codedText, CodeWords);
        }

        private Dictionary<char, int> GetFrequencyDictionary(string message)
        {
            var frequencyDict = new Dictionary<char, int>();

            foreach (char symbol in message)
            {
                if (frequencyDict.ContainsKey(symbol))
                {
                    frequencyDict[symbol]++;
                }
                else
                {
                    frequencyDict.Add(symbol, 1);
                }
            }

            return frequencyDict;
        }

        private Dictionary<char, string> GetCodesWhenTraversing(Node node)
        {
            Dictionary<char, string> codes = new Dictionary<char, string>(113);
            StringBuilder path = new StringBuilder(32);
            DoRecursionTraversal(node, path, codes);
            return codes;
        }

        private void DoRecursionTraversal(Node node, StringBuilder path, Dictionary<char, string> codes)
        {
            if (node.Childs != null)
            {
                for (int i = 0; i < node.Childs.Count; i++)
                {
                    path.Append((char)('0' + i));
                    DoRecursionTraversal(node.Childs[i], path, codes);
                    path.Remove(path.Length - 1, 1);
                }
            }
            else
            {
                codes.Add(node.Symbol, path.ToString());
            }
        }

        private class Node : IComparable<Node>
        {
            public char Symbol { get; private set; }
            public int Value { get; private set; }
            public IReadOnlyList<Node> Childs { get; private set; }
            public Node(char symbol, int value, List<Node> nodes)
            {
                this.Symbol = symbol;
                this.Value = value;
                Childs = nodes;
            }

            public int CompareTo(Node other)
            {
                var result = -this.Value.CompareTo(other.Value);
                if (result == 0)
                {
                    result = -this.Symbol.CompareTo(other.Symbol);
                }
                return result;
            }
        }
    }
}