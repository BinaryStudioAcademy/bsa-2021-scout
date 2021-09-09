using System.IO;
using System.Linq;
using entities = Domain.Entities;

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

        public entities::FileInfo ToFileInfo()
        {
            string[] linkFragments = Link.Substring(0, 8).Split("/");

            return new entities::FileInfo
            {
                Name = FileName,
                Path = string.Join("/", linkFragments.Skip(3).Take(linkFragments.Length - 5)),
                PublicUrl = Link,
            };
        }
    }
}
