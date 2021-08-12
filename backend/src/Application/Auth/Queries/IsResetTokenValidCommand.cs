using Application.Auth.Dtos;
using Application.Auth.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Auth.Queries
{
    public class IsResetTokenValidQuery : IRequest<bool>
    {
        public string Email { get; }
        public string ResetToken { get; }
        public IsResetTokenValidQuery(ResetTokenDto resetTokenDto)
        {
            Email = resetTokenDto.Email;
            ResetToken = resetTokenDto.Token;
        }
    }

    public class IsResetTokenValidQueryHandler : IRequestHandler<IsResetTokenValidQuery, bool>
    {
        protected readonly IReadRepository<User> _userReadRepository;

        public IsResetTokenValidQueryHandler(IReadRepository<User> userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<bool> Handle(IsResetTokenValidQuery query, CancellationToken _)
        {
            var user = await _userReadRepository.GetByPropertyAsync(nameof(User.Email), query.Email);

            if (user is null || user.ResetPasswordToken != query.ResetToken)
            {
                return false;
            }

            return true;
        }
    }
}
