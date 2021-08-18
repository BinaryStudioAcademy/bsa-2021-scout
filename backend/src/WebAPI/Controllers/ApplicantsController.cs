using Application.Common.Commands;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Application.ElasticEnities.CommandQuery.AddTagCommand;
using Application.ElasticEnities.CommandQuery.DeleteTagCommand;
using Application.Applicants.Queries;
using Application.ElasticEnities.Dtos;
using System.Threading;
using Domain.Entities;
using Application.Applicants.Commands;

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

        [HttpGet("to_tags/{searchRequest}")]
        public async Task<IActionResult> SearchElasticAsync(string searchRequest, CancellationToken token)
        {
            var query = new GetElasticDocumentsListBySearchRequestQuery<ElasticEnitityDto>(searchRequest, token);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicantAsync([FromBody] CreateApplicantDto createDto)
        {
           var query = new CreateComposedApplicantCommand(createDto);

            return Ok(await Mediator.Send(query));
        }
        
        [HttpPost("to_tags/")]
        public async Task<IActionResult> PostElasticAsync([FromBody] CreateElasticEntityDto createDto)
        {
            var query = new CreateElasticDocumentCommand<CreateElasticEntityDto>(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("csv/")]
        public async Task<IActionResult> PostApplicantFromCsv()
        {
            var file = Request.Form.Files[0];

            using (var fileReadStream = file.OpenReadStream())
            {
                var command = new CreateApplicantsFromCsvCommand(fileReadStream);

                return Ok(await Mediator.Send(command));
            }
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

        [HttpPut]
        public async Task<IActionResult> PutApplicantAsync([FromBody] UpdateApplicantDto updateDto)
        {
            var query = new UpdateComposedApplicantCommand(updateDto);

            return Ok(await Mediator.Send(query));
        }
        [HttpPut("to_tags/")]
        public async Task<IActionResult> PutElasticAsync([FromBody] UpdateApplicantToTagsDto updateDto)
        {
            var query = new UpdateElasticDocumentCommand<UpdateApplicantToTagsDto>(updateDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteApplicantAsync(string id)
        {
            var query = new DeleteEntityCommand(id);
            await Mediator.Send(query);

            var elasticQuery = new DeleteElasticDocumentCommand(id);
            await Mediator.Send(elasticQuery);

            return StatusCode(204);
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