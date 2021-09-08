namespace Application.Common.Files
{
    public class FileExtension
    {
        private FileExtension(string value) { Value = value; }

        public string Value { get; private set; }
        public static FileExtension Pdf { get => new FileExtension(".pdf"); }
    }
}
