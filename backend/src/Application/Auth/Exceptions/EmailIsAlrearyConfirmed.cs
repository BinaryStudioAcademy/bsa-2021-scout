using Newtonsoft.Json;
using System;

namespace Application.Auth.Exceptions
{
    public class EmailIsAlreadyConfirmed : Exception
    {
        public EmailIsAlreadyConfirmed() :
            base(JsonConvert.SerializeObject(new
            {
                type = "EmailIsAlreadyConfirmed",
                description = "Email is alreary confirmed"
            }))
        { }
    }
}
