namespace AmpedBiz.Service.Host.Auth.Tokenizers
{
    public interface ITokenizer<T> 
    {
        string Encode(T identity);
        T Decode(string token);
    }
}
