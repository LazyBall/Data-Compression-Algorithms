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
            var message = "swiss_miss";
            //var message = ReadFromFile("WarAndWorld.txt").Substring(2980000);
            //var message = ReadFromFile("test.txt");
            //message = message.Replace("\r\n", "\n");
            Console.WriteLine(message.Length);

            var list1 = new List<ITextEncodingAlgorithm>
            {
                new HuffmanCoding(),
                new RLE(),
                new LZ78(),
                new BWT()
            };

            var list2 = new List<ITextEncodingAlgorithm>
            {
                new HuffmanCoding(),
                new RLE(),
                new LZ78(),
                new BWT()
            };

            for(int i=0; i<list1.Count; i++)
            {
                var codedMessage = list1[i].Encode(message, out double ratio);
                var decodedMessage = list2[i].Decode(codedMessage);
                Console.Write(message == decodedMessage);
                Console.WriteLine(ratio);
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