using Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Handlers
{
    public class DeleteApplicantCommandHandler : DeleteEntityCommandHandler<Applicant>
    {
        public DeleteApplicantCommandHandler(IWriteRepository<Applicant> repository)
            : base(repository)
        { }
    }
}