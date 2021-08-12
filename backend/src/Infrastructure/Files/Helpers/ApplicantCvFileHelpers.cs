namespace Infrastructure.Files.Helpers
{
    public static class ApplicantCvFileHelpers
    {
        public static string GetFilePath()
        {
            return "applicants";
        }

        public static string GetFileName(string applicantId)
        {
            return $"{applicantId}-cv.pdf";
        }
    }
}
