using Application.Common.Commands;
using Application.Common.Exceptions;
using Application.ElasticEnities.Dtos;
using Application.Interfaces;
using Application.Interviews.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interviews.Commands.Create
{
    public class UpdateInterviewCommand : IRequest<InterviewDto>
    {
        public UpdateInterviewDto Interview { get; }

        public UpdateInterviewCommand(UpdateInterviewDto interview)
        {
            Interview = interview;
        }
    }

    public class UpdateInterviewCommandHandler : IRequestHandler<UpdateInterviewCommand, InterviewDto>
    {
        protected readonly IWriteRepository<Interview> _repository;
        protected readonly IWriteRepository<UsersToInterview> _usersToInterviewrepository;
        protected readonly IReadRepository<UsersToInterview> _usersToInterviewReadrepository;

        protected readonly IInterviewReadRepository _interviewReadRepository;

        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public UpdateInterviewCommandHandler(IWriteRepository<Interview> repository,
            IWriteRepository<UsersToInterview> usersToInterviewrepository,
            IInterviewReadRepository interviewReadRepository,
            IReadRepository<UsersToInterview> usersToInterviewReadrepository,
            ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _usersToInterviewrepository = usersToInterviewrepository;
            _interviewReadRepository = interviewReadRepository;
            _usersToInterviewReadrepository = usersToInterviewReadrepository;
        }

        public async Task<InterviewDto> Handle(UpdateInterviewCommand command, CancellationToken _)
        {
            var existedInterview = await _interviewReadRepository.GetAsync(command.Interview.Id);
            if (existedInterview is null)
            {
                throw new NotFoundException(nameof(Interview));
            }

            Interview entity = _mapper.Map<Interview>(command.Interview);
            entity.CreatedDate = existedInterview.CreatedDate;
            entity.CompanyId = existedInterview.CompanyId;
            entity.IsReviewed = true;

            var users = (await _usersToInterviewReadrepository.GetEnumerableAsync()).Where(x => x.InterviewId == command.Interview.Id);
            if (users.Any())
            {
                foreach (var user in users)
                {
                    await _usersToInterviewrepository.DeleteAsync(user.Id);
                }
            }
            
            foreach (var user in entity.UserParticipants)
            {
                await _usersToInterviewrepository.CreateAsync(user);
            }
            var created = await _repository.UpdateAsync(entity);
            return _mapper.Map<InterviewDto>(created);
        }
    }
}
