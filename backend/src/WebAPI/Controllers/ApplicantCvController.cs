using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
            ActionResult fileError = ValidateFileType(dto.File, "application/pdf");

            if (fileError != null)
            {
                return fileError;
            }

            byte[] bytes = GetFileBytes(dto.File);
            var command = new StartApplicantCvTextDetectionCommand(bytes);
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost("webhook/text-parsed")]
        [AllowAnonymous]
        public async Task<ActionResult> StartParsingFileToApplicant([FromBody] SnsNotificationDto data)
        {
            if (data.SubscribeURL != null)
            {
                Console.WriteLine($"SubscribeURL: {data.SubscribeURL}");
                return Ok();
            }

            TextractNotificationDto message = JsonConvert.DeserializeObject<TextractNotificationDto>(data.Message);
            var command = new StartParsingCvFileToApplicantCommand(message.JobId);
            await Mediator.Send(command);

            return Ok();
        }

        [HttpPost("webhook/skills-parsed")]
        [AllowAnonymous]
        public async Task<ActionResult> FinishParsingFileToApplicant([FromBody] SnsNotificationDto data)
        {
            if (data.SubscribeURL != null)
            {
                Console.WriteLine($"SubscribeURL: {data.SubscribeURL}");
                return Ok();
            }

            S3NotificationDto message = JsonConvert.DeserializeObject<S3NotificationDto>(data.Message);
            var command = new FinishParsingCvFileToApplicantCommand(message.Records.First().S3.Object.Key);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
