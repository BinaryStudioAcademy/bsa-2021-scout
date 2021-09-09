using System.Collections.Generic;
using Application.Reviews.Dtos;
using Domain.Enums;
using FluentValidation;

namespace Application.Stages.Dtos
{
    public class StageUpdateDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string VacancyId { get; set; }
        public string? DataJson { get; set; }
        public ICollection<ActionDto> Actions { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; }
    }

    public class StageUpdateDtoValidator : AbstractValidator<StageUpdateDto>
    {
        public StageUpdateDtoValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
            RuleFor(_ => _.Type).NotNull().NotEmpty();
            RuleFor(_ => _.Index).NotNull().NotEmpty();
            RuleFor(_ => _.IsReviewable).NotNull().NotEmpty();
            RuleFor(_ => _.VacancyId).NotNull().NotEmpty();
            RuleFor(_ => _.Actions).NotNull().NotEmpty();
        }
    }
}