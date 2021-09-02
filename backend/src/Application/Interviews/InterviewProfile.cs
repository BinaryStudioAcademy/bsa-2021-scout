using System.Linq;
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
            CreateMap<Interview, InterviewDto>()
            .ForMember(dest => dest.UserParticipants,
                opt => opt.MapFrom(src => src.UserParticipants.Select(ui => ui.User))
            );
            CreateMap<CreateInterviewDto, Interview>() 
            .ForMember(dest => dest.UserParticipants,
                opt => opt.MapFrom(src => src.UserParticipants.Select(ui => new UsersToInterview()
                {
                    UserId = ui.Id
                }))
            );
        }
    }
}