using Application.Interviews.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Interviews
{
    public class InterviewProfile: Profile
    {
        public InterviewProfile()
        {
            CreateMap<InterviewDto, Interview>().ReverseMap();
            CreateMap<CreateInterviewDto, Interview>();
        }
    }
}