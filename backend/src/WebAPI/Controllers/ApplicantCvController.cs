using System;
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
        [AllowAnonymous]
        public async Task<ActionResult> StartParsingFileToApplicant([FromForm] ApplicantCvOnlyFileDto dto)
        {
            string userId = "65cf0eb1-6d09-4711-add6-3628d369645a";
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
        [AllowAnonymous]
        public async Task<ActionResult> ParseFileToApplicant([FromBody] SnsNotificationDto data)
        {
            if (data.SubscribeURL != null)
            {
                Console.WriteLine($"SubscribeURL: {data.SubscribeURL}");
                return Ok();
            }

            Console.WriteLine(data.Message);
            TextractNotificationDto message = JsonConvert.DeserializeObject<TextractNotificationDto>(data.Message);
            var command = new ParseCvFileToApplicantCommand(message.JobId);
            await Mediator.Send(command);

            return Ok();
        }
    }
}
