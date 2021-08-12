using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public ResetPasswordDto ResetPasswordInfo { get; }
        public ResetPasswordCommand(ResetPasswordDto forgotPasswordDto)
        {
            ResetPasswordInfo = forgotPasswordDto;
        }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        protected readonly IUserReadRepository _userReadRepository;
        protected readonly IWriteRepository<User> _userWriteRepository;
        protected readonly ISecurityService _securityService;

        public ResetPasswordCommandHandler(IUserReadRepository userReadRepository, IWriteRepository<User> userWriteRepository,
                                            ISecurityService securityService)
        {
            _userReadRepository = userReadRepository;
            _userWriteRepository = userWriteRepository;
            _securityService = securityService;
        }

        public async Task<Unit> Handle(ResetPasswordCommand command, CancellationToken _)
        {
            var user = await _userReadRepository.GetByEmailAsync(command.ResetPasswordInfo.Email);

            if (user is null)
            {
                throw new NotFoundException(nameof(User));
            }

            if (user.ResetPasswordToken == null || user.ResetPasswordToken != command.ResetPasswordInfo.Token)
            {
                throw new InvalidTokenException("reset");
            }

            var salt = _securityService.GetRandomBytes();
            user.PasswordSalt = Convert.ToBase64String(salt);
            user.Password = _securityService.HashPassword(command.ResetPasswordInfo.Password, salt);
            user.ResetPasswordToken = null;
            await _userWriteRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
