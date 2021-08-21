using Application.Applicants.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Commands.DeleteApplicant
{
    public class DeleteApplicantCommand : IRequest<ApplicantDto>
    {
        public string Id { get; set; }

        public DeleteApplicantCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand, ApplicantDto>
    {
        private readonly IApplicantReadRepository _applicantReadRepository;
        private readonly IWriteRepository<Applicant> _applicantWriteRepository;
        private readonly IApplicantCvFileWriteRepository _applicantCvFileWriteRepository;
        private readonly IMapper _mapper;

        public DeleteApplicantCommandHandler(
            IApplicantReadRepository applicantReadRepository,
            IWriteRepository<Applicant> applicantWriteRepository,
            IApplicantCvFileWriteRepository applicantCvFileWriteRepository,
            IMapper mapper
        )
        {
            _applicantReadRepository = applicantReadRepository;
            _applicantWriteRepository = applicantWriteRepository;
            _applicantCvFileWriteRepository = applicantCvFileWriteRepository;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(DeleteApplicantCommand command, CancellationToken _)
        {
            var applicant = await _applicantReadRepository.GetByIdAsync(command.Id);

            await _applicantWriteRepository.DeleteAsync(applicant.Id);

            await DeleteCvFileIfExists(applicant);

            var deletedApplicantDto = _mapper.Map<ApplicantDto>(applicant);

            return deletedApplicantDto;
        }

        private async Task DeleteCvFileIfExists(Applicant applicant)
        {
            if (applicant.CvFileInfo == null)
            {
                return;
            }

            await _applicantCvFileWriteRepository.DeleteAsync(applicant.CvFileInfo);
        }
    }
}
