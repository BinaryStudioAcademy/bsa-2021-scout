using Application.SelfApply.Dtos;
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

namespace Application.SelfApply.Queries
{
    public class GetSelfApplyTokenQuery : IRequest<IEnumerable<string>>
    {
        public string VacancyId { get; }
        public string Email { get; }

        public GetSelfApplyTokenQuery(string vacancyId, string email)
        {
            VacancyId = vacancyId;
            Email = email;
        }
    }

    public class GetSelfApplyTokenQueryHandler : IRequestHandler<GetSelfApplyTokenQuery, IEnumerable<string>>
    {
        protected readonly IReadRepository<ApplyToken> _repository;
        protected readonly IMapper _mapper;

        public GetSelfApplyTokenQueryHandler(IReadRepository<ApplyToken> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> Handle(GetSelfApplyTokenQuery query, CancellationToken _)
        {
            var result = await _repository.GetEnumerableAsync();
            
            foreach(var res in result)
            {
                if (res.IsActive)
                {
                    res.IsActive = DateTime.Now < res.Expires;
                }
            }

            return result.Where(token => token.Email == query.Email && token.VacancyId == query.VacancyId && token.IsActive)
                         .Select(token => token.Token);
        }
    }
}
