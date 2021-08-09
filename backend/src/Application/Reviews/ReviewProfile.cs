using AutoMapper;
using Domain.Entities;
using Application.Reviews.Dtos;

namespace Application.Reviews
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDto>();
        }
    }
}
