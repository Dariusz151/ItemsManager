namespace ItemsManager.Users.Domain.Services
{
    public interface IEncrypter
    {
        string CreateSalt(int size);
        string GenerateSaltedHash(byte[] plainText, byte[] salt);
    }
}
