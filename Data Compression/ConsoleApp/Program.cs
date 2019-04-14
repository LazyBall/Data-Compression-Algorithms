using System;
using Data_Compression;

namespace ConsoleApp
{
    class Program
    {
        static void Main()
        {
            var messsage = Console.ReadLine();
            //var huf2 = new HuffmanCoding(5);
            //var code=huf2.Encode(messsage);
            //var decode=huf2.Decode(code);
            //Console.WriteLine("Huffman");
            //Console.WriteLine(code);
            //Console.WriteLine(decode);
            //Console.WriteLine();

            //Console.WriteLine("Arithmetic");
            //var ar = new ArithmeticCoding();
            //code=ar.Encode(messsage);
            //decode = ar.Decode(code);
            //Console.WriteLine(code);
            //Console.WriteLine(decode);
            var bw = new BWT();
            Console.WriteLine(bw.Encode(messsage));
        }
    }
}