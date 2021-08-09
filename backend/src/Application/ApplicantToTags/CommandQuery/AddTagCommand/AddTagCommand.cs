using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using entities = Domain.Entities;
using Domain.Interfaces;
using Application.ApplicantToTags.Dtos;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System;

namespace Application.ApplicantToTags.CommandQuery.AddTagCommand
{
    public class AddTagCommand : IRequest
    {
        public TagDto Tag { get; }
        public string ApplicantId {get; }

        public AddTagCommand(string applicantId, TagDto tag)
        {
            Tag = tag;
            tag.Id = Guid.NewGuid().ToString();
            ApplicantId = applicantId;
        }
    }

    public class AddTagCommandHandler : IRequestHandler<AddTagCommand>
    {
        protected readonly IElasticWriteRepository<entities::ApplicantToTags> _repository;
        protected readonly IElasticReadRepository<entities::ApplicantToTags> _readRepo;
        protected readonly IMapper _mapper;

        public AddTagCommandHandler(IElasticReadRepository<entities::ApplicantToTags> readRepo, IElasticWriteRepository<entities::ApplicantToTags> repository, IMapper mapper)
        {
            _repository = repository;
            _readRepo = readRepo;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddTagCommand command, CancellationToken _)
        {
            Tag tag = _mapper.Map<Tag>(command.Tag);
            IList<Tag> tagArray = (await _readRepo.GetAsync(command.ApplicantId)).Tags.ToList();
            tagArray.Add(tag);
            dynamic updateObject = new ExpandoObject();
            updateObject.tags = tagArray;
            await _repository.UpdateAsyncPartially(command.ApplicantId, updateObject);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
