using Application.Common.Commands;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using Application.ApplicantToTags.Dtos;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Application.ApplicantToTags.CommandQuery.AddTagCommand;
using Application.ApplicantToTags.CommandQuery.DeleteTagCommand;
using Application.Applicants.Queries;


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
        public async Task<IActionResult> SearchElasticAsync(string searchRequest)
        {
            var query = new GetElasticDocumentsListBySearchRequestQuery<ApplicantToTagsDto>(searchRequest);

            return Ok(await Mediator.Send(query));
        }
        [HttpPost]
        public async Task<IActionResult> PostApplicantAsync([FromBody] CreateApplicantDto createDto)
        {
            var newApplicant = _mapper.Map<ApplicantDto>(createDto);
            var query = new CreateEntityCommand<ApplicantDto>(newApplicant);

            return Ok(await Mediator.Send(query));
        }
        
        [HttpPost("to_tags/")]
        public async Task<IActionResult> PostElasticAsync([FromBody] CreateApplicantToTagsDto createDto)
        {
            var query = new CreateElasticDocumentCommand<CreateApplicantToTagsDto>(createDto);

            return Ok(await Mediator.Send(query));
        }
        [HttpPost("tags/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostTagAsync(string applicantId, [FromBody] TagDto createDto)
        {
            var query = new AddTagCommand(applicantId, createDto);
            return StatusCode(204, await Mediator.Send(query));
        }
        [HttpPost("to_tags/bulk")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PostElasticBulkAsync([FromBody] IEnumerable<CreateApplicantToTagsDto> createDtoList)
        {
            var query = new CreateBulkElasticDocumentCommand<CreateApplicantToTagsDto>(createDtoList);

            return StatusCode(204, await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutApplicantAsync([FromBody] UpdateApplicantDto updateDto)
        {
            var updatedApplicant = _mapper.Map<ApplicantDto>(updateDto);
            var query = new UpdateEntityCommand<ApplicantDto>(updatedApplicant);

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