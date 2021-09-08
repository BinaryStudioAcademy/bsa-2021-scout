using AutoMapper;
using Domain.Entities;
using Application.Arhive.Dtos;
using System;

namespace Application.Vacancies
{
    public class ArchiveProfile : Profile
    {
        public ArchiveProfile()
        {
            CreateMap<ArchivedEntity, ArchivedEntityDto>().ReverseMap();
            CreateMap<Vacancy, ArchivedVacancyDto>();
            CreateMap<Tuple<Vacancy, ArchivedEntity, ArchivedEntity>, ArchivedVacancyDto>()
                .BeforeMap<InnerVacancyMapAction>()
                .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(p => p.Item1.Project.Name))
                .ForMember(dest => dest.ArchivedVacancyData, opt => opt.MapFrom(p => p.Item2))
                .ForMember(dest => dest.IsProjectArchived, opt => opt.MapFrom(p => p.Item3 != null ? true : false));

            CreateMap<Project, ArchivedProjectDto>();
            CreateMap<Vacancy, ArchivedVacancyShortDto>();
            CreateMap<Tuple<Project, ArchivedEntity>, ArchivedProjectDto>()
                .BeforeMap<InnerProjectMapAction>()
                .ForMember(dest => dest.ArchivedProjectData, opt => opt.MapFrom(p => p.Item2));
        }

        public class InnerVacancyMapAction : IMappingAction<Tuple<Vacancy, ArchivedEntity, ArchivedEntity>, ArchivedVacancyDto>
        {
            public void Process(Tuple<Vacancy, ArchivedEntity, ArchivedEntity> opt, ArchivedVacancyDto dest, ResolutionContext context)
            {
                context.Mapper.Map<Vacancy, ArchivedVacancyDto>(opt.Item1, dest);
            }
        }

        public class InnerProjectMapAction : IMappingAction<Tuple<Project, ArchivedEntity>, ArchivedProjectDto>
        {
            public void Process(Tuple<Project, ArchivedEntity> opt, ArchivedProjectDto dest, ResolutionContext context)
            {
                context.Mapper.Map<Project, ArchivedProjectDto>(opt.Item1, dest);
            }
        }
    }
}
