using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.ApplicantCvs.Dtos;
using Application.ApplicantCvs.Commands;

namespace WebAPI.Controllers
{
    public class ApplicantCvController : ApiController
    {
        [HttpPost("file-to-applicant")]
        public async Task<ActionResult> ParseFileToApplicant([FromForm] ApplicantCvOnlyFileDto dto)
        {
            string userId = GetUserIdFromToken();
            ActionResult fileError = ValidateFileType(dto.File, "application/pdf");

            if (fileError != null)
            {
                return fileError;
            }

            byte[] bytes = GetFileBytes(dto.File);
            var command = new StartApplicantCvTextDetectionCommand(bytes, userId);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
