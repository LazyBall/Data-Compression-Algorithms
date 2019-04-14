﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Data_Compression
{
    /// <summary>
    /// Huffman coding
    /// </summary>
    // source http://neerc.ifmo.ru/wiki/index.php?title=Алгоритм_Хаффмана_для_n_ичной_системы_счисления
    // http://trinary.ru/kb/d0085167-8832-466b-8cba-cc2e377e84c3
    public class HuffmanCoding : ITextEncodingAlgorithm
    {
        private readonly int _numberSystem;

        public HuffmanCoding(int numberSystem)
        {
            if (numberSystem < 2) throw new ArgumentException();
            this._numberSystem = numberSystem;
        }

        private IReadOnlyDictionary<char, string> CreateCodeWords(IReadOnlyDictionary<char, double> 
            frequencyDictionary, int numberSystem)
        {
            var tree = new List<Node>(frequencyDictionary.Count);

            foreach (var key in frequencyDictionary.Keys)
            {
                tree.Add(new Node(key, frequencyDictionary[key], null));
            }

            int m = frequencyDictionary.Count;
            if (m > numberSystem)
            {

                while ((m - numberSystem) % (numberSystem - 1) != 0)
                {
                    m++;
                }

            }
            int start = numberSystem - (m - frequencyDictionary.Count); //сколько нужно взять на первом шаге

            // первый шаг
            tree.Sort();
            var nodes = new List<Node>(start);
            var sumNodes = 0.0;
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
                var list = new List<Node>(numberSystem);
                var sum = 0.0;

                while (list.Count < numberSystem)
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

        private IReadOnlyDictionary<char, string> CreateCodeWords(string message, int numberSystem)
        {
            return CreateCodeWords(GetFrequencyDictionary(message), numberSystem);
        }

        public string Encode(string sourceText)
        {
            return Coder.GetCodedMessage(sourceText, CreateCodeWords(sourceText, _numberSystem), true);
        }

        public string Decode(string codedText)
        {
            return Decoder.GetDecodedMessage(codedText);
        }

        private IReadOnlyDictionary<char, double> GetFrequencyDictionary(string message)
        {
            var frequencyDict = new Dictionary<char, double>();

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

        private IReadOnlyDictionary<char, string> GetCodesWhenTraversing(Node node)
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
            public double Value { get; private set; }
            public IReadOnlyList<Node> Childs { get; private set; }

            public Node(char symbol, double value, List<Node> childs)
            {
                this.Symbol = symbol;
                this.Value = value;
                this.Childs = childs;
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

        private static class Coder
        {

            public static string GetCodedMessage(string message, IReadOnlyDictionary<char, string> codes,
                bool addCodes = false)
            {
                var codedMessage = new StringBuilder(GetCodedMessage(message, codes));

                if (addCodes)
                {
                    codedMessage.Append('{');

                    foreach (var pair in codes)
                    {
                        codedMessage.Append(string.Format("[{0}-{1}]", pair.Key, pair.Value));
                    }

                    codedMessage.Append('}');
                }

                return codedMessage.ToString();
            }

            static string GetCodedMessage(string message, IReadOnlyDictionary<char, string> codes)
            {
                StringBuilder codedMessage = new StringBuilder(message.Length * 3);

                foreach (var symbol in message)
                {
                    codedMessage.Append(codes[symbol]);
                }

                return codedMessage.ToString();
            }

        }

        private static class Decoder
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
}