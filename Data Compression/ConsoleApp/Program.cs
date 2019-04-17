using System;
using Data_Compression;
using System.IO;
using System.Collections.Generic;


namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            //var message = @"sdhbsduhP}}{}{}{}O{OIUH(986737432462_[3-5-6--]][65-1][a-002]";
            var message = ReadFromFile("WarAndWorld.txt").Substring(2980000);
            //var message = ReadFromFile("test.txt");
            //message = message.Replace("\r\n", "\n");
            Console.WriteLine(message.Length);

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

            for(int i=0; i<list1.Count; i++)
            {
                var codedMessage = list1[i].Encode(message);
                var decodedMessage = list2[i].Decode(codedMessage);
                Console.WriteLine(message == decodedMessage);
            }
            
        }

        static string ReadFromFile(string fileName)
        {
            using (var inputfile = new StreamReader(fileName, System.Text.Encoding.Default))
            {
                return inputfile.ReadToEnd();
            }
        }
    }
}