
namespace Data_Compression
{
    public interface ICodingAlgorithm
    {
        string Encode(string sourceText);
        string Decode(string codedText);
    }
}