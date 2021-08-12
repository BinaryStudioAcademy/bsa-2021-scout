using Application.Common.Commands;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Application.ElasticEnities.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Application.ElasticEnities.CommandQuery.AddTagCommand;
using Application.ElasticEnities.CommandQuery.DeleteTagCommand;
using System.Threading;

namespace WebAPI.Controllers
{
    public class ApplicantsController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicantAsync(string id)
        {
            var query = new GetEntityByIdQuery<ApplicantDto>(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicantsAsync()
        {
            var query = new GetEntityListQuery<ApplicantDto>();

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
            var query = new CreateEntityCommand<CreateApplicantDto>(createDto);

            return Ok(await Mediator.Send(query));
        }
        
        [HttpPost("to_tags/")]
        public async Task<IActionResult> PostElasticAsync([FromBody] CreateElasticEntityDto createDto)
        {
            var query = new CreateElasticDocumentCommand<CreateElasticEntityDto>(createDto);

            return Ok(await Mediator.Send(query));
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
            var query = new UpdateEntityCommand<UpdateApplicantDto>(updateDto);

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

            return StatusCode(204, await Mediator.Send(query));
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