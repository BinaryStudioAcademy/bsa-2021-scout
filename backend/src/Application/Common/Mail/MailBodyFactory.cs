namespace Application.Common.Mail
{
    public static class MailBodyFactory
    {
        // public static readonly string BODY_1 = "...some html...";
        public static readonly string CV_PARSED = "Your CV parsing is finished. Visit <a href='{0}'>this link</a> to finish applicant creation";
        public static readonly string CONFIRM_EMAIL = "Hello!<br>To confirm you registration, follow the <a href='{{LINK}}'>link</a><br>If you have not registered an account on Scout, please ignore this message.";
        public static readonly string FORGOT_PASSWORD = "Hello!<br>To reset the password for your Scout account, follow the <a href='{{CALLBACK}}'>link</a><br>If you have not requested a password change, please ignore this message.";
        public static readonly string BODY_REGISTRATION_LINK = "Hello!<br>To register your Scout account, follow the <a href='{{CALLBACK}}'>link</a><br>If you have not requested a registration, please ignore this message.";
        public static readonly string CONFIRM_APPLY_VACANCY = "Hello!<br>To confirm your applying for vacancy {{VACANCY}}, follow the <a href='{{CALLBACK}}'>link</a><br>If you have not requested this letter, please ignore this message.";
    }
}
