namespace ItemsManager.Users.Domain.Services
{
    public interface IEncrypter
    {
        byte[] CreateSalt(int size);
        byte[] GenerateSaltedHash(byte[] plainText, byte[] salt);
    }
}
