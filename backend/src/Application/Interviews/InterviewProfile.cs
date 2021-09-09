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
            ).ForMember(dest=> dest.Candidate, 
                opt => opt.MapFrom(src => src.Candidate)
            )
            ;
            CreateMap<CreateInterviewDto, Interview>() 
            .ForMember(dest => dest.UserParticipants,
                opt => opt.MapFrom(src => src.UserParticipants.Select(ui => new UsersToInterview()
                {
                    UserId = ui
                }))
            );
            CreateMap<UpdateInterviewDto, Interview>()
            .ForMember(dest => dest.UserParticipants,
                opt => opt.MapFrom(src => src.UserParticipants.Select(ui => new UsersToInterview()
                {
                    UserId = ui
                }))
            );
        }
    }
}