namespace Application.Interfaces
{
    public interface ISmtpFactory
    {
        ISmtp Connect(string address, string password, string displayName);
    }
}
