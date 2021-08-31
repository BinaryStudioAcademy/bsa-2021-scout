using Domain.Interfaces.Read;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Applicants.Dtos;
using AutoMapper;

namespace Application.Applicants.Queries
{
    public class GetApplicantByPropertyQuery : IRequest<ApplicantDto>
    {
        public string Property { get; }
        public string PropertyName { get; }

        public GetApplicantByPropertyQuery(string property, string propertyName)
        {
            Property = property;
            PropertyName = propertyName;
        }
    }

    public class GetApplicantExistingByEmailQueryHandler : IRequestHandler<GetApplicantByPropertyQuery, ApplicantDto>
    {
        protected readonly IApplicantReadRepository _repository;
        protected readonly IMapper _mapper;

        public GetApplicantExistingByEmailQueryHandler(IApplicantReadRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(GetApplicantByPropertyQuery query, CancellationToken _)
        {
            var result = await _repository.GetByPropertyAsync(query.PropertyName, query.Property);

            return _mapper.Map<ApplicantDto>(result);
        }
    }
}
