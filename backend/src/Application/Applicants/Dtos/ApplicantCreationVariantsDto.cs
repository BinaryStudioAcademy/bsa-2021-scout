using System.Collections.Generic;

namespace Application.Applicants.Dtos
{
    public class ApplicantCreationVariantsDto
    {
        public IEnumerable<string> FirstName { get; set; }
        public IEnumerable<string> LastName { get; set; }
        public IEnumerable<string> Experience { get; set; }
        public IEnumerable<string> Phone { get; set; }
        public IEnumerable<string> Skype { get; set; }
        public IEnumerable<string> Email { get; set; }
        public IEnumerable<string> Skills { get; set; }
        public IEnumerable<string> Company { get; set; }
        public IEnumerable<string> BirthDate { get; set; }
        public string Cv { get; set; }
    }
}
