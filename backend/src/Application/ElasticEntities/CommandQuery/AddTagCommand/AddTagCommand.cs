using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using entities = Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.ElasticEnities.Dtos;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System;

namespace Application.ElasticEnities.CommandQuery.AddTagCommand
{
    public class AddTagCommand : IRequest
    {
        public TagDto Tag { get; }
        public string EntityId { get; }

        public AddTagCommand(string entityId, TagDto tag)
        {
            Tag = tag;
            tag.Id = Guid.NewGuid().ToString();
            EntityId = entityId;
        }
    }

    public class AddTagCommandHandler : IRequestHandler<AddTagCommand>
    {
        protected readonly IElasticWriteRepository<entities::ElasticEntity> _repository;
        protected readonly IElasticReadRepository<entities::ElasticEntity> _readRepo;
        protected readonly IMapper _mapper;

        public AddTagCommandHandler(IElasticReadRepository<entities::ElasticEntity> readRepo, IElasticWriteRepository<entities::ElasticEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _readRepo = readRepo;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddTagCommand command, CancellationToken _)
        {
            Tag tag = _mapper.Map<Tag>(command.Tag);
            IList<Tag> tagArray = (await _readRepo.GetAsync(command.EntityId)).Tags.ToList();
            tagArray.Add(tag);
            dynamic updateObject = new ExpandoObject();
            updateObject.tags = tagArray;
            await _repository.UpdateAsyncPartially(command.EntityId, updateObject);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
