using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;
using Domain.Enums;
using Application.Common.Models;
using Application.Reviews.Dtos;

namespace Application.Stages.Dtos
{
    public class StageCreateDto
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string? DataJson { get; set; }
        public ICollection<ActionCreateDto> Actions { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
    }

    public class StageCreateDtoValidator : AbstractValidator<StageCreateDto>
    {
        public StageCreateDtoValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
            RuleFor(_ => _.Type).NotNull().NotEmpty();
            RuleFor(_ => _.Index).NotNull().NotEmpty();
            RuleFor(_ => _.IsReviewable).NotNull().NotEmpty();

            RuleFor(_ => _.Actions).NotNull().NotEmpty();
        }
    }
}