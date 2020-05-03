namespace Shared.Utility.Security
{
    public interface IAesCryptoEngine
    {
        string DeCrypt(string dataToDecrypt);
        string Encrypt(string dataToEncrypt);
    }
}