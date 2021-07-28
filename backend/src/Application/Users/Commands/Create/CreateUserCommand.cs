using Application.Users.Dtos;
using Application.ApplicantCv.Dtos;
using Application.Common.Commands;
using Domain.Interfaces;
using Domain.Entities;
using e = Domain.Entities;
using AutoMapper;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandHandler : CreateEntityCommandHandler<User, UserDto>
    {
        public CreateUserCommandHandler(IWriteRepository<User> repository, IMapper mapper) : base(repository, mapper) { }
    }

    public class CreateCvCmdH : CreateEntityCommandHandler<e::ApplicantCv, ApplicantCvDto>
    {
        public CreateCvCmdH(IWriteRepository<e::ApplicantCv> repository, IMapper mapper) : base(repository, mapper) { }
    }
}
