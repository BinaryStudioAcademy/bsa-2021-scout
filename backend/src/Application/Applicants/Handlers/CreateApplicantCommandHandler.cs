using AutoMapper;
using Domain.Entities;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Handlers
{
    public class CreateApplicantCommandHandler : CreateEntityCommandHandler<Applicant, ApplicantDto>
    {
        public CreateApplicantCommandHandler(IWriteRepository<Applicant> repository, IMapper mapper)
            : base(repository, mapper)
        { }
    }
}