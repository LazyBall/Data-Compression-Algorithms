using DataCompressionAlgorithms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var message = CreateRandomString(1000);
            //var message = ReadFromFile("WarAndWorld.txt");
            //message = message.Substring(message.Length - message.Length / 100000);
            //var message = ReadFromFile("test.txt");
            //message = message.Replace("\r\n", "\n");
            Console.WriteLine("Length: {0}", message.Length);

            var list1 = new List<ITextEncodingAlgorithm>
            {
                new HuffmanCoding(),
                new RLE(),
                new LZ78(),
                new BWT(),
                new ArithmeticCoding()
            };

            var list2 = new List<ITextEncodingAlgorithm>
            {
                new HuffmanCoding(),
                new RLE(),
                new LZ78(),
                new BWT(),
                new ArithmeticCoding()
            };

            for (int i = 0; i < list1.Count; i++)
            {
                var codedMessage = list1[i].Encode(message, out double ratio);
                var decodedMessage = list2[i].Decode(codedMessage);
                Console.WriteLine();
                Console.WriteLine(list1[i].ToString());
                Console.WriteLine(message == decodedMessage);
                Console.WriteLine(ratio);
                Console.WriteLine();
            }
        }

        static string ReadFromFile(string fileName)
        {
            using (var inputfile = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                return inputfile.ReadToEnd();
            }
        }

        static string CreateRandomString(int length)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(DateTime.Now.Millisecond);

            while (length > 0)
            {
                builder.Append((char)random.Next(65000));
                length--;
            }

            return builder.ToString();
        }
    }
}