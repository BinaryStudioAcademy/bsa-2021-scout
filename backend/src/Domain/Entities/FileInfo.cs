using Domain.Common;

namespace Domain.Entities
{
    public class FileInfo : Entity
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string PublicUrl { get; set; }
    }
}