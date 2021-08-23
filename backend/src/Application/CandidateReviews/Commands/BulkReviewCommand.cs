using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Domain.Entities;
using Domain.Interfaces.Write;
using Domain.Interfaces.Abstractions;
using Application.CandidateReviews.Dtos;

namespace Application.CandidateReviews.Commands
{
    public class BulkReviewCommand : IRequest
    {
        public BulkReviewDto Data { get; set; }

        public BulkReviewCommand(BulkReviewDto data)
        {
            Data = data;
        }
    }

    public class BulkReviewCommandHandler : IRequestHandler<BulkReviewCommand>
    {
        private readonly ICandidateReviewWriteRepository _repository;
        private readonly IWriteRepository<CandidateComment> _commentRepository;

        public BulkReviewCommandHandler(
            ICandidateReviewWriteRepository repository,
            IWriteRepository<CandidateComment> commentRepository
        )
        {
            _repository = repository;
            _commentRepository = commentRepository;
        }

        public async Task<Unit> Handle(BulkReviewCommand command, CancellationToken _)
        {
            List<CandidateReview> list = new List<CandidateReview>();

            foreach (KeyValuePair<string, int> pair in command.Data.Data)
            {
                CandidateReview candidateReview = new CandidateReview
                {
                    StageId = command.Data.StageId,
                    CandidateId = command.Data.CandidateId,
                    ReviewId = pair.Key,
                    Mark = pair.Value,
                };

                list.Add(candidateReview);
            }

            await _repository.BulkCreateAsync(list);

            if (!string.IsNullOrWhiteSpace(command.Data.Comment))
            {
                CandidateComment comment = new CandidateComment
                {
                    StageId = command.Data.StageId,
                    CandidateId = command.Data.CandidateId,
                    Text = command.Data.Comment,
                };

                await _commentRepository.CreateAsync(comment);
            }

            return Unit.Value;
        }
    }
}
