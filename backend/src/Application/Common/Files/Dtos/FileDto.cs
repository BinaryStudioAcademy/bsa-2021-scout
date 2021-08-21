using System.IO;

namespace Application.Common.Files.Dtos
{
    public class FileDto
    {
        public Stream Content { get; }
        public string FileName { get; }

        public FileDto(Stream content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }
    }
}
