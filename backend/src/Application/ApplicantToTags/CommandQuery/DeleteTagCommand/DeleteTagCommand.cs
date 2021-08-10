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

namespace Application.ApplicantToTags.CommandQuery.DeleteTagCommand
{
    public class DeleteTagCommand : IRequest
    {
        public string TagId { get; }
        public string ApplicantId {get; }

        public DeleteTagCommand(string applicantId, string tagId)
        {
            TagId = tagId;
            ApplicantId = applicantId;
        }
    }

    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand>
    {
        protected readonly IElasticWriteRepository<entities::ApplicantToTags> _repository;
        protected readonly IElasticReadRepository<entities::ApplicantToTags> _readRepo;
        protected readonly IMapper _mapper;

        public DeleteTagCommandHandler(IElasticReadRepository<entities::ApplicantToTags> readRepo, IElasticWriteRepository<entities::ApplicantToTags> repository, IMapper mapper)
        {
            _repository = repository;
            _readRepo = readRepo;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteTagCommand command, CancellationToken _)
        {
            IList<Tag> tagArray = (await _readRepo.GetAsync(command.ApplicantId)).Tags.ToList();
            Tag deleteTag = tagArray.FirstOrDefault<Tag>(tag=>tag.Id == command.TagId);
            tagArray.Remove(deleteTag);
            
            dynamic updateObject = new ExpandoObject();
            updateObject.tags = tagArray;
            await _repository.UpdateAsyncPartially(command.ApplicantId, updateObject);

            return await Task.FromResult<Unit>(Unit.Value);
        }
    }
}
