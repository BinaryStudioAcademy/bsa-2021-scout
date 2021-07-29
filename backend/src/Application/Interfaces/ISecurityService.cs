
namespace Application.Interfaces
{
    public interface ISecurityService
    {
        string HashPassword(string password, byte[] salt);
        bool ValidatePassword(string password, string hash, string salt);
        byte[] GetRandomBytes(int length = 32);
    }
}
