using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Application.Common.Mail;
using Application.Interfaces;
using Application.Mail;
using Application.Users.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
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
        protected readonly IReadRepository<RegisterPermission> _registerPermissionReadRepository;
        protected readonly IWriteRepository<RegisterPermission> _registerPermissionWriteRepository;

        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public RegisterUserCommandHandler(ISender mediator, IWriteRepository<User> userWriteRepository,
                                   IReadRepository<RegisterPermission> registerPermissionReadRepository,
                                   IWriteRepository<RegisterPermission> registerPermissionWriteRepository,
                                   ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _userWriteRepository = userWriteRepository;
            _registerPermissionReadRepository = registerPermissionReadRepository;
            _registerPermissionWriteRepository = registerPermissionWriteRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(RegisterUserCommand command, CancellationToken _)
        {
            var registerPermission = await _registerPermissionReadRepository.GetByPropertyAsync(nameof(RegisterPermission.Email), command.RegisterDto.UserRegisterDto.Email);
            if (registerPermission is null || !registerPermission.IsActive)
            {
                throw new InvalidTokenException("register");
            }

            command.RegisterDto.UserRegisterDto.Roles.Add(new RoleDto { Id = "2", Name = "HrUser" });
            var newUser = _mapper.Map<User>(command.RegisterDto.UserRegisterDto);
            newUser.CompanyId = registerPermission.CompanyId;
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
                MailSubjectFactory.CONFIRM_EMAIL,
                MailBodyFactory.CONFIRM_EMAIL);
            await _mediator.Send(sendConfirmEmailMailCommand);

            await _registerPermissionWriteRepository.DeleteAsync(registerPermission.Id);

            return Unit.Value;

        }
    }
}