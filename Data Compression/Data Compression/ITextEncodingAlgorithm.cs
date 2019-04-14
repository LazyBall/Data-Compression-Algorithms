
namespace Data_Compression
{    
    public interface ITextEncodingAlgorithm
    {
        string Encode(string sourceText);
        string Decode(string codedText);
    }
}