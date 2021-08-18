using Application.Auth.Commands;
using Application.Auth.Dtos;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.Commands
{
    public class RegisterUserCommand : IRequest<Unit>
    {
        public RegisterDto RegisterDto { get; }

        public RegisterUserCommand(RegisterDto registerDto)
        {
            RegisterDto = registerDto;
        }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Unit>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<User> _userWriteRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public RegisterUserCommandHandler(ISender mediator, IWriteRepository<User> userWriteRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _userWriteRepository = userWriteRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken _)
        {
            var newUser = _mapper.Map<User>(command.RegisterDto.UserRegisterDto);
            newUser.CompanyId = "1"; // !IMPORTANT! delete in the future         

            newUser.IsEmailConfirmed = false;
            var salt = _securityService.GetRandomBytes();
            newUser.PasswordSalt = Convert.ToBase64String(salt);
            newUser.Password = _securityService.HashPassword(command.RegisterDto.UserRegisterDto.Password, salt);

            await _userWriteRepository.CreateAsync(newUser);

            var registeredUser = _mapper.Map<UserDto>(newUser);
            registeredUser.Roles = command.RegisterDto.UserRegisterDto.Roles;

            var sendConfirmEmailMailCommand = new SendConfirmEmailMailCommand(
                registeredUser,
                command.RegisterDto.ClientUrl,
                MailSubjectFactory.confirmEmailMailSubject,
                MailBodyFactory.confirmEmailMailBody);
            await _mediator.Send(sendConfirmEmailMailCommand);

            return Unit.Value;

        }
    }
}