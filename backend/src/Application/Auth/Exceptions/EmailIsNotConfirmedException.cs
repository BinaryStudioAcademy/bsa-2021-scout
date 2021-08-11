using Newtonsoft.Json;
using System;

namespace Application.Auth.Exceptions
{
    public class EmailIsNotConfirmedException : Exception
    {
        public EmailIsNotConfirmedException() :
            base(JsonConvert.SerializeObject(new
            {
                type = "EmailIsNotConfirmed",
                description = "Email is not confirmed"
            }))
        { }
    }
}
