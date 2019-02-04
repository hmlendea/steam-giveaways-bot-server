namespace SteamAccountDistributor.Security
{
    public interface IHmacEncoder
    {
        string GenerateToken<T>(T obj, string sharedSecretKey) where T : class;

        bool IsTokenValid<T>(string expectedToken, T obj, string sharedSecretKey) where T : class;
    }
}
