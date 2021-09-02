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

namespace Application.ApplicantCsvs.Queries
{
    public class GetCsvFilesEnumerableQuery : IRequest<IEnumerable<CsvFileDto>>
    { }
    public class GetCsvFilesEnumerableQueryHandler : IRequestHandler<GetCsvFilesEnumerableQuery, IEnumerable<CsvFileDto>>
    {
        protected readonly IReadRepository<CsvFile> _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public GetCsvFilesEnumerableQueryHandler(IReadRepository<CsvFile> repository,
            ICurrentUserContext currentUserContext,
            IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CsvFileDto>> Handle(GetCsvFilesEnumerableQuery query, CancellationToken _)
        {
            var user = await _currentUserContext.GetCurrentUser();

            var result = await _repository.GetEnumerableAsync();

            return _mapper.Map<IEnumerable<CsvFileDto>>(result.Where(file => file.User.CompanyId == user.CompanyId));
        }
    }
}
