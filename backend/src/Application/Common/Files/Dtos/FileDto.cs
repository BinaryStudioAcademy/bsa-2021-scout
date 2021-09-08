using System.IO;
using System.Linq;

namespace Application.Common.Files.Dtos
{
    public class FileDto
    {
        public Stream Content { get; set; }
        public string FileName { get; set; }
        public string Link { get; set; }

        public FileDto(Stream content, string fileName)
        {
            Content = content;
            FileName = fileName;
        }

        public FileDto(string link)
        {
            Link = link;
            FileName = link.Split("/").LastOrDefault();
        }
    }
}
