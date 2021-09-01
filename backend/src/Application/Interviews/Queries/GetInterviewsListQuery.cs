using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Interviews.Dtos;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Interviews.Queries

{
    public class GetInterviewsListQuery : IRequest<IEnumerable<InterviewDto>>
    {
    }

    public class GetInterviewsListQueryHandler : IRequestHandler<GetInterviewsListQuery, IEnumerable<InterviewDto>>
    {
        protected readonly IReadRepository<Interview> _interviewRepository;

        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public GetInterviewsListQueryHandler(IReadRepository<Interview> interviewRepository,
                                   ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _interviewRepository = interviewRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<InterviewDto>> Handle(GetInterviewsListQuery command, CancellationToken _)
        {
            var currUser = await _currentUserContext.GetCurrentUser();

            if (currUser is null)
                throw new Exception("There is no such user");

            var interviews = await _interviewRepository.GetEnumerableAsync();
            
            return _mapper.Map<List<InterviewDto>>(interviews);
        }
    }
}