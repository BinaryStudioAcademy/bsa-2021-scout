namespace Application.Common.Mail
{
    public static class MailBodyFactory
    {
        // public static readonly string BODY_1 = "...some html...";
        public static readonly string CV_PARSED = "Your CV parsing is finished. Visit <a href='{0}'>this link</a> to finsish applicant creation";
        public static readonly string CONFIRM_EMAIL = "Hello!<br>To confirm you registration, follow the <a href='{{LINK}}'>link</a><br>If you have not registered an account on Scout, please ignore this message.";
        public static readonly string FORGOT_PASSWORD = "Hello!<br>To reset the password for your Scout account, follow the link<br>{{CALLBACK}}<br>If you have not requested a password change, please ignore this message.";
    }
}
