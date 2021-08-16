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

namespace Application.Auth.Commands
{
    public class GenerateEmailTokenCommand : IRequest<EmailToken>
    {
        public UserDto User { get; }

        public GenerateEmailTokenCommand(UserDto user)
        {
            User = user;
        }
    }

    public class GenerateEmailTokenCommandHandler : IRequestHandler<GenerateEmailTokenCommand, EmailToken>
    {
        protected readonly IReadRepository<EmailToken> _tokenReadRepository;
        protected readonly IWriteRepository<EmailToken> _tokenWriteRepository;

        protected readonly IJwtService _jwtService;

        public GenerateEmailTokenCommandHandler(IReadRepository<EmailToken> tokenReadRepository,
                                   IWriteRepository<EmailToken> tokenWriteRepository, IJwtService jwtService
                                  )
        {
            _tokenReadRepository = tokenReadRepository;
            _tokenWriteRepository = tokenWriteRepository;
            _jwtService = jwtService;
        }

        public async Task<EmailToken> Handle(GenerateEmailTokenCommand command, CancellationToken _)
        {

            await _tokenWriteRepository.DeleteAsync((await _tokenReadRepository.GetByPropertyAsync("UserId", command.User.Id))?.Id); // delete the token we've exchanged

            var emailToken = await _jwtService.GenerateJsonWebToken(command.User);
            return (EmailToken)await _tokenWriteRepository.CreateAsync(new EmailToken()
            {
                Token = emailToken.Token,
                UserId = command.User.Id
            });
        }
    }
}
