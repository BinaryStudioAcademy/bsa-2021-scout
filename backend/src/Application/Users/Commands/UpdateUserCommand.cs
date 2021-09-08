using Application.Users.Dtos;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Users.Queries;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Files.Dtos;
using Domain.Interfaces.Write;
using Domain.Interfaces.Read;

namespace Application.Users.Commands.Create
{
    public class UpdateUserCommand : IRequest<UserDto>
    {
        public UserUpdateDto User { get; }
        public FileDto? ImgFileDto { get; set; }


        public UpdateUserCommand(UserUpdateDto user, FileDto? imgFileDto)
        {
            ImgFileDto = imgFileDto;
            User = user;
        }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        protected readonly IWriteRepository<User> _writeRepository;
        protected readonly IUserReadRepository _readRepository;
        protected readonly ICurrentUserContext _currentUser;
        protected readonly IImageWriteRepository _imageWriteRepository;


        protected readonly IMapper _mapper;
        private readonly ISender _mediator;

        public UpdateUserCommandHandler(IWriteRepository<User> writeRepository,
            IImageWriteRepository imageWriteRepository,
            IUserReadRepository readRepository, IMapper mapper, ISender mediator, ICurrentUserContext currentUser)
        {
            _imageWriteRepository = imageWriteRepository;
            _writeRepository = writeRepository;
            _readRepository = readRepository;
            _mapper = mapper;
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<UserDto> Handle(UpdateUserCommand command, CancellationToken _)
        {
            UserDto currentUser = await _currentUser.GetCurrentUser();

            List<UserDto> selectedUsers = new List<UserDto>();

            if (currentUser.Roles.Any(x => x.Name == "HrLead"))
            {
                var query = new GetUsersForHrLeadQuery();
                selectedUsers = (await _mediator.Send(query)).ToList();
            }
            
            if (currentUser.Id == command.User.Id || selectedUsers.Any(x=>x.Id == command.User.Id))
            {
                User userToUpdate = await _readRepository.GetByIdAsync(command.User.Id);
                User entity = _mapper.Map<User>(command.User);

                userToUpdate.BirthDate = entity.BirthDate;
                userToUpdate.FirstName = entity.FirstName;
                userToUpdate.LastName = entity.LastName;
                userToUpdate.Phone = entity.Phone;
                userToUpdate.Skype = entity.Skype;
                userToUpdate.Slack = entity.Slack;

                await UploadAvatarFileIfExists(userToUpdate, command);

                var updated = await _writeRepository.UpdateAsync(userToUpdate);

                return _mapper.Map<UserDto>(updated);
            }

            throw new System.Exception("The user was not found or the current user has no rights to edit this user.");

        }
        private async Task UploadAvatarFileIfExists(User user, UpdateUserCommand command)
        {
            if (command.User.IsImageToDelete == true)
            {
                user.AvatarId = null;
                await _writeRepository.UpdateAsync(user);
                await _imageWriteRepository.DeleteAsync(user.Avatar);
            }

            if (command.ImgFileDto == null)
            {
                return;
            }

            FileInfo uploadedImage;
            if(user.Avatar != null)
            {
                await _imageWriteRepository.UpdateAsync(user.Id, command.ImgFileDto!.Content);
            }
            else
            {
                uploadedImage = await _imageWriteRepository.UploadAsync(user.Id, command.ImgFileDto!.Content);
                user.Avatar = uploadedImage;
            }
            
        }
    }
}
