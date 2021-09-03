using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Commands
{
    public class RevokeRegistrationLinkCommand : IRequest<Unit>
    {
        public string Id { get; private set; }

        public RevokeRegistrationLinkCommand(string id)
        {
            Id = id;
        }
    }

    public class RevokeRegistrationLinkCommandHandler : IRequestHandler<RevokeRegistrationLinkCommand, Unit>
    {
        private readonly IWriteRepository<RegisterPermission> _registerPermissionWriteRepository;
       
        public RevokeRegistrationLinkCommandHandler(IWriteRepository<RegisterPermission> registerPermissionWriteRepository)
        {
            _registerPermissionWriteRepository = registerPermissionWriteRepository;
        }

        public async Task<Unit> Handle(RevokeRegistrationLinkCommand command, CancellationToken _)
        {
            await _registerPermissionWriteRepository.DeleteAsync(command.Id);
            return Unit.Value;
        }
    }
}
