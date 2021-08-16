using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Vacancies.Commands.Create
{
    //public class CreateVacancyCommandHandler : CreateEntityCommandHandler<Vacancy, VacancyCreateDto>
    //{
    //    public CreateVacancyCommandHandler(IWriteRepository<Vacancy> repository, IMapper mapper) : base(repository, mapper) { }
    //}
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

            public CreateVacancyCommandHandler(
                IWriteRepository<Vacancy> writeRepository,
                IMapper mapper
            )
            {
                _writeRepository = writeRepository;
                _mapper = mapper;
            }

            public async Task<VacancyDto> Handle(CreateVacancyCommand command, CancellationToken _)
            {
                var newUser = _mapper.Map<Vacancy>(command.VacancyCreate);

                await _writeRepository.CreateAsync(newUser);
                var registeredUser = _mapper.Map<VacancyDto>(newUser);

                return registeredUser;
            }
        }
    }
