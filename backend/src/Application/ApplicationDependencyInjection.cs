using MediatR;
using FluentValidation;
using System.Reflection;
using entities = Domain.Entities;
using System.Collections.Generic;
using Application.Common.Queries;
using Application.Common.Commands;
using Application.Applicants.Dtos;
using Application.Common.Behaviours;
using Application.ApplicantToTags.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.RegisterAbstractQueriesEntityTypes();
            services.RegisterAbstractCommandsEntityTypes();

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddPipelineBehaviour();

            return services;
        }

        private static IServiceCollection AddPipelineBehaviour(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }

        // This two following methods are used only before AddMediatR, otherwise MediatR will not work correctly
        private static IServiceCollection RegisterAbstractQueriesEntityTypes(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestHandler<GetEntityListQuery<ApplicantDto>, IEnumerable<ApplicantDto>>),
                typeof(GetEntityListQuery<entities.Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<GetEntityByIdQuery<ApplicantDto>, ApplicantDto>),
                typeof(GetEntityByIdQueryHandler<entities.Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<GetElasticDocumentByIdQuery<ApplicantToTagsDto>, ApplicantToTagsDto>),
                typeof(GetElasticDocumentByIdQueryHandler<entities.ApplicantToTags, ApplicantToTagsDto>));

            return services;
        }

        private static IServiceCollection RegisterAbstractCommandsEntityTypes(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<ApplicantDto>, ApplicantDto>),
                typeof(CreateEntityCommandHandler<entities.Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<UpdateEntityCommand<ApplicantDto>, ApplicantDto>),
                typeof(UpdateEntityCommandHandler<entities.Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<DeleteEntityCommand, Unit>),
                typeof(DeleteEntityCommandHandler<entities.Applicant>));

            return services;
        }
    }
}
