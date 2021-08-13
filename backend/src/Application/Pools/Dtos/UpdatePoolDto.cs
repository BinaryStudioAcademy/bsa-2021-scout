using System;
using System.Collections.Generic;
using Application.Common.Models;
using FluentValidation;

namespace Application.Pools.Dtos
{
    public class UpdatePoolDto : Dto
    {
        public string Name { get; set; }
        public string Description { get; set; }        
        public string ApplicantsIds { get; set; }
    }

    public class UpdatePoolDtoValidator : AbstractValidator<UpdatePoolDto>
    {
        public UpdatePoolDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty();
            RuleFor(a => a.Description).NotNull().NotEmpty();
            
        }
    }
}