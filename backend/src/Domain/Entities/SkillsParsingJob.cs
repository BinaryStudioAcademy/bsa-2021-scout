using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class SkillsParsingJob : Job
    {
        public string OriginalFilePath { get; set; }
        public string TextPath { get; set; }
        public string OutputPath { get; set; }
    }
}
