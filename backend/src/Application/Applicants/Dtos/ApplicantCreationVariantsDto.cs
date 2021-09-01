using System.Collections.Generic;
using Newtonsoft.Json;

namespace Application.Applicants.Dtos
{
    public class ApplicantCreationVariantsDto
    {
        [JsonProperty("firstName")]
        public IEnumerable<string> FirstName { get; set; }

        [JsonProperty("lastName")]
        public IEnumerable<string> LastName { get; set; }

        [JsonProperty("experience")]
        public IEnumerable<string> Experience { get; set; }

        [JsonProperty("phone")]
        public IEnumerable<string> Phone { get; set; }

        [JsonProperty("email")]
        public IEnumerable<string> Email { get; set; }

        [JsonProperty("skills")]
        public IEnumerable<string> Skills { get; set; }

        [JsonProperty("company")]
        public IEnumerable<string> Company { get; set; }

        [JsonProperty("birthDate")]
        public IEnumerable<string> BirthDate { get; set; }

        [JsonProperty("cv")]
        public string Cv { get; set; }
    }
}
