using AutoMapper;
using Domain.Entities;
using Application.Tasks.Dtos;
using System.Linq;

namespace Application.Tasks
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskDto, ToDoTask>();
            CreateMap<Applicant, TeamMemberstDto>();
            CreateMap<User, TeamMemberstDto>();


            CreateMap<ToDoTask, TaskDto>()
                .ForMember(dest => dest.TeamMembers, opt => opt.MapFrom(src => src.TeamMembers.Select(p => p.User)))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name));
            //        .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));               

            CreateMap<UpdateTaskDto, ToDoTask>();                
            CreateMap<ToDoTask, UpdateTaskDto>();



        }
    }
}