using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using MediatR;
using Domain.Entities;
using Domain.Interfaces.Write;
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

        public BulkReviewCommandHandler(ICandidateReviewWriteRepository repository)
        {
            _repository = repository;
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
            }

            await _repository.BulkCreateAsync(list);

            return Unit.Value;
        }
    }
}
