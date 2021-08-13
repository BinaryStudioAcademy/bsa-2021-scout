using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Auth
{
    public class EmailTokenProfile : Profile
    {
        public EmailTokenProfile()
        {
            CreateMap<EmailToken, EmailTokenDto>();
        }
    }
}
