using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Interfaces;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Queries;

namespace Application.Auth.Commands
{
    public class ComfirmUserEmailCommand : IRequest<AuthUserDto>
    {
        public ConfirmEmailDto EmailToken { get; }


        public ComfirmUserEmailCommand(ConfirmEmailDto emailToken)
        {
            EmailToken = emailToken;
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
            var user = await _userReadRepository.GetByPropertyAsync("Email", command.EmailToken.Email);
            if (user.IsEmailConfirmed)
            {
                throw new EmailIsAlreadyConfirmed();
            }
            var getEmailTokenByPropertyQuery = new GetEntityByPropertyQuery<EmailTokenDto>("UserId", user.Id);
            var token = _mapper.Map<EmailTokenDto>(await _mediator.Send(getEmailTokenByPropertyQuery));
            if (token?.Token == command.EmailToken.Token)
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
            else
            {
                throw new InvalidTokenException("email confirmation");
            }
        }
    }
}
