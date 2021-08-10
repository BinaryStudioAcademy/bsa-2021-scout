using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Applicants.Dtos;
using Application.ApplicantCvs.Dtos;
using Application.ApplicantCvs.Commands;

namespace WebAPI.Controllers
{
    public class ApplicantCvController : ApiController
    {
        [HttpPost("file-to-applicant")]
        public async Task<ActionResult<ApplicantDto>> ParseFileToApplicant([FromForm] ApplicantCvOnlyFileDto dto)
        {
            byte[] bytes = GetFileBytes(dto.File);
            var command = new ParseCvFileToApplicantCommand(bytes, dto.File.ContentType);

            return Ok(await Mediator.Send(command));
        }
    }
}
