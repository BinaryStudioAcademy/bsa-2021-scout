using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth.Dtos
{
    public class EmailTokenDto : Dto
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
