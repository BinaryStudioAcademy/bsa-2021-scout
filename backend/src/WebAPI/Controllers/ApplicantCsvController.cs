using Application.ApplicantCsvs.Commands;
using Application.ApplicantCsvs.Dtos;
using Application.ApplicantCsvs.Queries;
using Application.Applicants.Commands.CreateApplicant;
using Application.Applicants.Dtos;
using Application.Applicants.Queries;
using Application.Common.Files.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class ApplicantCsvController : ApiController
    {
        [HttpPost("file/")]
        public async Task<IActionResult> PostFile([FromForm] string body)
        {
            var command = new CreateCsvFileCommand(JsonConvert.DeserializeObject<CsvFileDto>(body));

            return Ok(await Mediator.Send(command));
        }

        [HttpPut("file/")]
        public async Task<IActionResult> PutFile([FromForm] string body)
        {
            var command = new UpdateCsvFileCommand(JsonConvert.DeserializeObject<CsvFileDto>(body));

            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetFilesAsync()
        {
            var query = new GetCsvFilesEnumerableQuery();

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("csv/")]
        public async Task<IActionResult> GetApplicantFromCsv()
        {
            var file = Request.Form.Files[0];

            using (var fileReadStream = file.OpenReadStream())
            {
                var command = new GetApplicantsFromCsvCommand(fileReadStream);

                return Ok(await Mediator.Send(command));
            }
        }

        [HttpPost("range/")]
        public async Task<IActionResult> GetApplicantFromCsv(IEnumerable<CreateApplicantDto> applicants)
        {
            var command = new CreateRangeOfApplicantsCommand(applicants);

            return Ok(await Mediator.Send(command));
        }


        [HttpPost("csvApplicant")]
        public async Task<IActionResult> PostCsvApplicantAsync([FromForm] string body, [FromForm] IFormFile cvFile = null)
        {
            var createApplicantDto = JsonConvert.DeserializeObject<CreateApplicantDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var query = new CreateCsvApplicantCommand(createApplicantDto!, cvFileDto);

            return Ok(await Mediator.Send(query));
        }

    }
}
