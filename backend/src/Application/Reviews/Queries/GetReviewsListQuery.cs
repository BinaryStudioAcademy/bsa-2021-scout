using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Application.Common.Queries;
using Application.Reviews.Dtos;

namespace Application.Reviews.Queries
{
    public class GetReviewsListQueryHandler : GetEntityListQueryHandler<Review, ReviewDto>
    {
        public GetReviewsListQueryHandler(IReadRepository<Review> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}
