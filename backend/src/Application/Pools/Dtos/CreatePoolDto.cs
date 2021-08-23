using Application.Common.Models;
using System;
using FluentValidation;
using System.Collections.Generic;

namespace Application.Pools.Dtos
{
    public class CreatePoolDto : Dto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> ApplicantsIds { get; set; }        
    }

    public class CreatePoolDtoValidator : AbstractValidator<CreatePoolDto>
    {
        public CreatePoolDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty();
            RuleFor(a => a.Description).NotNull().NotEmpty();            
        }
    }
}