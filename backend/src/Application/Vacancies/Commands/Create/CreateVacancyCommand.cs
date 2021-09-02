using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Common.Exceptions;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using Application.Stages.Commands;
using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Vacancies.Commands.Create
{
    public class CreateVacancyCommand : IRequest<VacancyDto>
    {
        public VacancyCreateDto VacancyCreate { get; set; }

        public CreateVacancyCommand(VacancyCreateDto vacancyCreate)
        {
            VacancyCreate = vacancyCreate;
        }
    }

    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, VacancyDto>
    {
        private readonly IWriteRepository<Vacancy> _writeRepository;
        private readonly IMapper _mapper;
        private readonly ISender _mediator;
        private readonly ICurrentUserContext _currUser;
        private readonly string DefaultColumnName = "Self-Applied";
        public CreateVacancyCommandHandler(
                IWriteRepository<Vacancy> writeRepository,
                IMapper mapper, ISender mediator, ICurrentUserContext currUser
            )
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
            _mediator = mediator;
            _currUser = currUser;
        }

        public async Task<VacancyDto> Handle(CreateVacancyCommand command, CancellationToken _)
        {
            var user = await _currUser.GetCurrentUser();
            if (user is null)
            {
                throw new NotFoundException(typeof(User), "unknown");
            }

            var newVacancy = _mapper.Map<Vacancy>(command.VacancyCreate);

            newVacancy.Stages.Add(new Stage
            {
                Name = DefaultColumnName,
                Type = StageType.Applied,
                Index = 0,
                IsReviewable = false,
                VacancyId = newVacancy.Id
            });

            newVacancy.CompanyId = user.CompanyId;
            newVacancy.ResponsibleHrId = user.Id;
            newVacancy.CreationDate = DateTime.UtcNow;
            newVacancy.DateOfOpening = newVacancy.CreationDate; // Must be changed in future
            newVacancy.ModificationDate = DateTime.UtcNow;


            var stagesWithReview = newVacancy.Stages.Where(x => x.ReviewToStages != null && x.ReviewToStages.Count() != 0);
            if (stagesWithReview.Count() !=0)
            {
                foreach (var stage in stagesWithReview)
                {
                    foreach (var reviewToStages in stage.ReviewToStages)
                    {
                        reviewToStages.Review = null;
                    }
                }
            }
            await _writeRepository.CreateAsync(newVacancy);

            var elasticQuery = new CreateElasticDocumentCommand<CreateElasticEntityDto>(new CreateElasticEntityDto()
            {
                ElasticType = ElasticType.ApplicantTags,
                Id = newVacancy.Id,
                TagsDtos = command.VacancyCreate.Tags.TagDtos.Select(t => new TagDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    TagName = t.TagName
                })
            });

            var registeredVacancy = _mapper.Map<VacancyDto>(newVacancy);
            registeredVacancy.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));
            return registeredVacancy;
        }
    }
}