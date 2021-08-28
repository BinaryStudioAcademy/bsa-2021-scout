using Application.Applicants.Commands;
using Application.Applicants.Commands.CreateApplicant;
using Application.Applicants.Commands.DeleteApplicant;
using Application.Applicants.Dtos;
using Application.Applicants.Queries;
using Application.Common.Commands;
using Application.Common.Files.Dtos;
using Application.Common.Queries;
using Application.ElasticEnities.CommandQuery.AddTagCommand;
using Application.ElasticEnities.CommandQuery.DeleteTagCommand;
using Application.ElasticEnities.Dtos;
using Application.VacancyCandidates.Commands;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class ApplicantsController : ApiController
    {
        protected IMapper _mapper;
        public ApplicantsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicantAsync(string id)
        {
            var query = new GetComposedApplicantQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicantsAsync()
        {
            var query = new GetComposedApplicantListQuery();

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("company/{id}")]
        public async Task<IActionResult> GetApplicantByCompanyAsync(string id)
        {
            var query = new GetApplicantByIdByCompanyQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("marked/{vacancyId}")]
        public async Task<IActionResult> GetApplicantsWithAppliedMark(string vacancyId)
        {
            var query = new GetApplicantsWithAppliedMark(vacancyId);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicantAsync([FromForm] string body, [FromForm] IFormFile cvFile = null)
        {
            var createApplicantDto = JsonConvert.DeserializeObject<CreateApplicantDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var query = new CreateApplicantCommand(createApplicantDto!, cvFileDto);

            return Ok(await Mediator.Send(query));
        }
        [HttpPost("self-apply/{vacancyId}")]
        public async Task<IActionResult> PostSelfAppliedApplicantAsync(string vacancyId, [FromForm] string body, [FromForm] IFormFile cvFile = null)
        {
            var createApplicantDto = JsonConvert.DeserializeObject<CreateApplicantDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var commandApplicant = new CreateSelfAppliedApplicantCommand(createApplicantDto!, cvFileDto, vacancyId);

            var applicant= await Mediator.Send(commandApplicant);

            var commandCandidate = new CreateVacancyCandidateNoAuthCommand(applicant.Id, vacancyId);

            return Ok(await Mediator.Send(commandCandidate));
        }

        [HttpPut]
        public async Task<IActionResult> PutApplicantAsync([FromForm] string body, [FromForm] IFormFile cvFile = null)
        {
            var updateApplicantDto = JsonConvert.DeserializeObject<UpdateApplicantDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var query = new UpdateApplicantCommand(updateApplicantDto!, cvFileDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteApplicantAsync(string id)
        {
            var query = new DeleteApplicantCommand(id);
            await Mediator.Send(query);

            var elasticQuery = new DeleteElasticDocumentCommand(id);
            await Mediator.Send(elasticQuery);

            return StatusCode(204);
        }

        [HttpGet("{id}/cv")]
        public async Task<IActionResult> GetApplicantCvAsync(string id)
        {
            var query = new GetApplicantCvUrlQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpPut("{id}/cv")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateApplicantCvAsync(string id, [FromForm] IFormFile cvFile)
        {
            var cvFileDto = new FileDto(cvFile.OpenReadStream(), cvFile.FileName);

            var query = new UpdateApplicantCvCommand(id, cvFileDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("to_tags/{searchRequest}")]
        public async Task<IActionResult> SearchElasticAsync(string searchRequest, CancellationToken token)
        {
            var query = new GetElasticDocumentsListBySearchRequestQuery<ElasticEnitityDto>(searchRequest, token);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet("property/{propertyName}/{property}")]
        public async Task<IActionResult> GetByPropertyAsync(string propertyName, string property)
        {
            var query = new GetApplicantByPropertyQuery(property, propertyName);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("to_tags/")]
        public async Task<IActionResult> PostElasticAsync([FromBody] CreateElasticEntityDto createDto)
        {
            var query = new CreateElasticDocumentCommand<CreateElasticEntityDto>(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("csv/")]
        public async Task<IActionResult> GetApplicantFromCsv()
        {
            var file = Request.Form.Files[0];

            using (var fileReadStream = file.OpenReadStream())
            {
                var command = new CetApplicantsFromCsvCommand(fileReadStream);

                return Ok(await Mediator.Send(command));
            }
        }

        [HttpPost("range/")]
        public async Task<IActionResult> GetApplicantFromCsv(IEnumerable<CreateApplicantDto> applicants)
        {
            var command = new CreateRangeOfApplicantsCommand(applicants);

            return Ok(await Mediator.Send(command));
        }

        [HttpPost("tags/{entityId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostTagAsync(string entityId, [FromBody] TagDto createDto)
        {
            var query = new AddTagCommand(entityId, createDto);
            return StatusCode(204, await Mediator.Send(query));
        }

        [HttpPost("to_tags/bulk")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostElasticBulkAsync([FromBody] IEnumerable<CreateElasticEntityDto> createDtoList)
        {
            var query = new CreateBulkElasticDocumentCommand<CreateElasticEntityDto>(createDtoList);

            return StatusCode(204, await Mediator.Send(query));
        }

        [HttpPut("to_tags/")]
        public async Task<IActionResult> PutElasticAsync([FromBody] UpdateApplicantToTagsDto updateDto)
        {
            var query = new UpdateElasticDocumentCommand<UpdateApplicantToTagsDto>(updateDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("to_tags/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteElasticAsync(string id)
        {
            var query = new DeleteElasticDocumentCommand(id);
            return StatusCode(204, await Mediator.Send(query));
        }

        [HttpDelete("tags")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteElasticAsync(string applicantId, string tagId)
        {
            var query = new DeleteTagCommand(applicantId, tagId);
            return StatusCode(204, await Mediator.Send(query));
        }
    }
}