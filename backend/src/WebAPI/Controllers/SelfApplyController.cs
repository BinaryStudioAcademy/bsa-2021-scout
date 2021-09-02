using Application.Applicants.Commands.CreateApplicant;
using Application.Applicants.Dtos;
using Application.Common.Files.Dtos;
using Application.SelfApply.Commands;
using Application.SelfApply.Dtos;
using Application.SelfApply.Queries;
using Application.VacancyCandidates.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class SelfApplyController : ApiController
    {
        [HttpPost("email-confirm-apply")]
        public async Task<IActionResult> SendApplyConfirmation(ApplyTokenDto applyTokenDto)
        {
            var command = new SendApplyConfirmEmailCommand(applyTokenDto);

            return Ok(await Mediator.Send(command));
        }

        [HttpPost("mark-token-used/{token}")]
        public async Task<IActionResult> MarkTokenUsed(string token)
        {
            var command = new MarkTokenAsUsedCommand(token);

            return Ok(await Mediator.Send(command));
        }

        [HttpGet("email-confirm-apply/{vacancyId}/{email}")]
        public async Task<IActionResult> GetApplyConfirmation(string vacancyId, string email)
        {
            var query = new GetSelfApplyTokenQuery(vacancyId, email);

            return Ok(await Mediator.Send(query));
        }

        [HttpPost("{vacancyId}")]
        public async Task<IActionResult> PostSelfAppliedApplicantAsync(string vacancyId, [FromForm] string body, [FromForm] IFormFile cvFile = null)
        {
            var createApplicantDto = JsonConvert.DeserializeObject<CreateApplicantDto>(body);

            var cvFileDto = cvFile != null ? new FileDto(cvFile.OpenReadStream(), cvFile.FileName) : null;

            var commandApplicant = new CreateSelfAppliedApplicantCommand(createApplicantDto!, cvFileDto, vacancyId);

            var applicant = await Mediator.Send(commandApplicant);

            var commandCandidate = new CreateVacancyCandidateNoAuthCommand(applicant.Id, vacancyId);

            return Ok(await Mediator.Send(commandCandidate));
        }
    }
}
