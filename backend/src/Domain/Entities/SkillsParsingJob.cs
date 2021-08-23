using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class SkillsParsingJob : Job
    {
        public string TextPath { get; set; }
        public string OutputPath { get; set; }
    }
}
