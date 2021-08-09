namespace Application.Common.Mail
{
    public static class MailBodyFactory
    {
        // public static readonly string BODY_1 = "...some html...";
        public static readonly string confirmEmailMailBody = "Hello!<br>To confirm the password for your Scout account, follow the link {{LINK}}<br>If you have not requested a password change, please ignore this message.";
    }
}
