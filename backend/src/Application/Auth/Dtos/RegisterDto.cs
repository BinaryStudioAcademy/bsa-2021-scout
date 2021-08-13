using Application.Common.Models;
using Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dtos
{
    public class RegisterDto
    {
        public UserRegisterDto UserRegisterDto { get; set; }

        public string ClientUrl { get; set; }

    }
}
