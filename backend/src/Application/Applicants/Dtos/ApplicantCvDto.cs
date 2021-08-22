namespace Application.Common.Files.Dtos
{
    public class ApplicantCvDto
    {
        public string Url { get; }

        public ApplicantCvDto(string url)
        {
            Url = url;
        }
    }
}
