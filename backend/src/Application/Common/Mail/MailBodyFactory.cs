namespace Application.Common.Mail
{
    public static class MailBodyFactory
    {
        // public static readonly string BODY_1 = "...some html...";
        public static readonly string confirmEmailMailBody = "Hello!<br>To confirm you registration, follow the <a href='{{LINK}}'>link</a><br>If you have not registered an account on Scout, please ignore this message.";

        public static readonly string BODY_FORGOT_PASSWORD = "Hello!<br>To reset the password for your Scout account, follow the link<br>{{CALLBACK}}<br>If you have not requested a password change, please ignore this message.";
    }
}
