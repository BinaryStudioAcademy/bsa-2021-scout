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
    public class CreateCsvFileCommand : IRequest<CsvFileDto>
    {
        public CsvFileDto CsvFile { get; }

        public CreateCsvFileCommand(CsvFileDto csvFile)
        {
            CsvFile = csvFile;
        }
    }

    public class CreateCsvFileCommandHandler : IRequestHandler<CreateCsvFileCommand, CsvFileDto>
    {
        private readonly IWriteRepository<CsvFile> _writeRepository;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMapper _mapper;
        public CreateCsvFileCommandHandler(IWriteRepository<CsvFile> writeRepository,
            ICurrentUserContext currentUserContext,
            IMapper mapper)
        {
            _writeRepository = writeRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<CsvFileDto> Handle(CreateCsvFileCommand command, CancellationToken _)
        {
            var user = await _currentUserContext.GetCurrentUser();

            var csvFile = _mapper.Map<CsvFile>(command.CsvFile);

            csvFile.DateAdded = DateTime.Now;
            csvFile.User = _mapper.Map<User>(user);

            var createdFile = _mapper.Map<CsvFileDto>(await _writeRepository.CreateAsync(csvFile));

            return createdFile;
        }
    }
}
