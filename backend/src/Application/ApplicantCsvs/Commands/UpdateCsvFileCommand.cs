using Application.ApplicantCsvs.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ApplicantCsvs.Commands
{
    public class UpdateCsvFileCommand : IRequest
    {
        public CsvFileDto CsvFile { get; }

        public UpdateCsvFileCommand(CsvFileDto csvFile)
        {
            CsvFile = csvFile;
        }
    }

    public class UpdateCsvFileCommandHandler : IRequestHandler<UpdateCsvFileCommand>
    {
        private readonly IWriteRepository<CsvFile> _writeRepository;
        private readonly IMapper _mapper;
        public UpdateCsvFileCommandHandler(IWriteRepository<CsvFile> writeRepository,
            IMapper mapper)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCsvFileCommand command, CancellationToken _)
        {
            var csvFile = new CsvFile
            {
                Id = command.CsvFile.Id,
                Name = command.CsvFile.Name,
                DateAdded = command.CsvFile.DateAdded,
                Json = command.CsvFile.Json,
                User = new User
                {
                    Id = command.CsvFile.User.Id,
                    FirstName = command.CsvFile.User.FirstName,
                    LastName = command.CsvFile.User.LastName,
                    CompanyId = command.CsvFile.User.CompanyId
                }
            };

            await _writeRepository.UpdateAsync(csvFile);

            return Unit.Value;
        }
    }
}
