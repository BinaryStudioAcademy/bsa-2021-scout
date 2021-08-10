using System;
using AutoMapper;
using Domain.Entities;
using Application.CandidateReviews.Dtos;

namespace Application.CandidateReviews
{
    public class CandidateReviewProfile : Profile
    {
        public CandidateReviewProfile()
        {
            CreateMap<CandidateReview, CandidateReviewShortDto>()
                .ForMember(dto => dto.ReviewName, opt => opt.MapFrom(cr => cr.Review.Name))
                .ForMember(
                    dto => dto.Mark,
                    opt => opt
                        .MapFrom(cr => Math.Max(Math.Min((int)Math.Ceiling(cr.Mark), 10), 0))
                );
        }
    }
}
