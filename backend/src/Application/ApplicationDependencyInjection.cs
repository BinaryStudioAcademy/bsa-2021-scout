using Application.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Common.Queries;
using Application.Common.Commands;
using Application.Applicants.Dtos;
using Domain.Entities;
using System.Collections.Generic;

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
                typeof(GetEntityListQuery<Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<GetEntityByIdQuery<ApplicantDto>, ApplicantDto>),
                typeof(GetEntityByIdQueryHandler<Applicant, ApplicantDto>));

            return services;
        }

        private static IServiceCollection RegisterAbstractCommandsEntityTypes(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestHandler<CreateEntityCommand<ApplicantDto>, ApplicantDto>),
                typeof(CreateEntityCommandHandler<Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<UpdateEntityCommand<ApplicantDto>, ApplicantDto>),
                typeof(UpdateEntityCommandHandler<Applicant, ApplicantDto>));
            services.AddScoped(typeof(IRequestHandler<DeleteEntityCommand, Unit>),
                typeof(DeleteEntityCommandHandler<Applicant>));

            return services;
        }
    }
}
