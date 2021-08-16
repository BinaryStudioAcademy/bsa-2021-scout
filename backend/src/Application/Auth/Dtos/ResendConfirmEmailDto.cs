using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dtos
{
    public class ResendConfirmEmailDto
    {
        public string Email { get; set; }

        public string ClientUrl { get; set; }

    }
}
