namespace Application.Common.Files
{
    public class FileExtension
    {
        private FileExtension(string value) { Value = value; }

        public string Value { get; private set; }
        public static FileExtension Pdf { get => new FileExtension(".pdf"); }
        public static FileExtension Png { get => new FileExtension(".png"); }
        public static FileExtension Jpg { get => new FileExtension(".jpg"); }
        public static FileExtension Jpeg { get => new FileExtension(".jpeg"); }
    }
}
