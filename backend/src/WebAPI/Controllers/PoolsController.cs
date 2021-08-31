using Application.Pools.Commands;
using Application.Common.Commands;
using Application.Common.Queries;
using Application.Pools.Dtos;
using Application.Pools.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Infrastructure.Services;
using Application.Interfaces;
using Application.Users.Dtos;
using Domain.Entities;

namespace WebAPI.Controllers
{
    public class PoolsController : ApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoolAsync(string id)
        {
            var query = new GetPoolWithApplicantsByIdQuery(id);

            return Ok(await Mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> GetPoolsAsync()
        {
            var query = new GetPoolsWithApplicantsQuery();

            return Ok(await Mediator.Send(query));
        }
        
        [HttpPost]
        public async Task<IActionResult> PostPoolAsync([FromBody] CreatePoolDto createDto)
        {
                        
            var query = new CreatePoolCommand(createDto);

            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<IActionResult> PutPooltAsync([FromBody] UpdatePoolDto updateDto)
        {
            var query = new UpdatePoolCommand(updateDto);

            return Ok(await Mediator.Send(query));
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeletePoolAsync(string id)
        {
            var query = new DeletePoolCommand(id);
            await Mediator.Send(query);
            
            return NoContent();

        }        
    }
}