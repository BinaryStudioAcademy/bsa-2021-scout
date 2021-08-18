using AutoMapper;
using Domain.Entities;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Handlers
{
    public class UpdateApplicantCommandHandler : UpdateEntityCommandHandler<Applicant, ApplicantDto>
    {
        public UpdateApplicantCommandHandler(IWriteRepository<Applicant> repository, IMapper mapper)
            : base(repository, mapper)
        { }
    }
}