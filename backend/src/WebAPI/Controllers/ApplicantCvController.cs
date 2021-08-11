using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Application.SNS.Dtos;
using Application.ApplicantCvs.Dtos;
using Application.ApplicantCvs.Commands;

namespace WebAPI.Controllers
{
    public class ApplicantCvController : ApiController
    {
        [HttpPost("file-to-applicant")]
        public async Task<ActionResult> StartParsingFileToApplicant([FromForm] ApplicantCvOnlyFileDto dto)
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

        [HttpPost("webhook/parsed")]
        public async Task<ActionResult> ParseFileToApplicant([FromBody] SnsNotificationDto data)
        {
            if (data.SubscribeURL != null)
            {
                Console.WriteLine($"SubscribeURL: {data.SubscribeURL}");
                return Ok();
            }

            TextractNotificationDto message = JsonConvert.DeserializeObject<TextractNotificationDto>(data.Message);
            var command = new ParseCvFileToApplicantCommand(message.JobId);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
