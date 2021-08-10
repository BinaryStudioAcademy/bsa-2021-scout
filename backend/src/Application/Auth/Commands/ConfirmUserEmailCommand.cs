using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Common.Queries;

namespace Application.Auth.Commands
{
    public class ComfirmUserEmailCommand : IRequest<AuthUserDto>
    {
        public string Email { get; }
        public string Token { get; }


        public ComfirmUserEmailCommand(string email, string token)
        {
            Email = email;

            Token = token;
        }
    }

    public class ComfirmUserEmailCommandHandler : IRequestHandler<ComfirmUserEmailCommand, AuthUserDto>
    {
        protected readonly IReadRepository<EmailToken> _tokenReadRepository;
        protected readonly IWriteRepository<EmailToken> _tokenWriteRepository;

        protected readonly IReadRepository<User> _userReadRepository;
        protected readonly IWriteRepository<User> _userWriteRepository;


        protected readonly ISender _mediator;
        protected readonly ISecurityService _securityService;

        protected readonly IMapper _mapper;

        public ComfirmUserEmailCommandHandler(IReadRepository<EmailToken> tokenReadRepository,
            IWriteRepository<EmailToken> tokenWriteRepository,
            IReadRepository<User> userReadRepository,
            IWriteRepository<User> userWriteRepository,
                                   ISender mediator,
                                   ISecurityService securityService,
                                   IMapper mapper)
        {
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;

            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;

            _mapper = mapper;

            _mediator = mediator;
            _securityService = securityService;
        }

        public async Task<AuthUserDto> Handle(ComfirmUserEmailCommand command, CancellationToken _)
        {
            var getUserByPropertyQuery = new GetEntityByPropertyQuery<UserDto>("Email",command.Email);
            var user = _mapper.Map<User>(await _mediator.Send(getUserByPropertyQuery));
            var token = await _tokenReadRepository.GetByPropertyAsync("UserId", user.Id);
            //var getEmailTokenByPropertyQuery = new GetEntityByPropertyQuery<EmailTokenDto>("UserId", user.Id);
            //var token = _mapper.Map<EmailToken>(await _mediator.Send(getEmailTokenByPropertyQuery));
            if (token?.Token == command.Token)
            {
                await _tokenWriteRepository.DeleteAsync(token.Id);

                user.IsEmailConfirmed = true;

                var upadatedUser = _mapper.Map<UserDto>(await _userWriteRepository.UpdateAsync(user));

                var generateTokenCommand = new GenerateAccessTokenCommand(upadatedUser);
                var generateToken = await _mediator.Send(generateTokenCommand);

                return new AuthUserDto
                {
                    User = upadatedUser,
                    Token = generateToken
                };
            }
            return null;
        }
    }
}
