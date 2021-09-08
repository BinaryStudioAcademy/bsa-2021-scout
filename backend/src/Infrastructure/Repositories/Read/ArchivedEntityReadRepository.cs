using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ArchivedEntityReadRepository : ReadRepository<ArchivedEntity>, IArchivedEntityReadRepository
    {
        public ArchivedEntityReadRepository(IConnectionFactory connectionFactory) : base("ArchivedEntities", connectionFactory) { }

        public async Task<ArchivedEntity> GetByEntityTypeAndIdAsync(EntityType entityType, string entityId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @$"SELECT AE.*
                            FROM ArchivedEntities AS AE        
                            WHERE AE.EntityType = @entityType 
		                      AND AE.EntityId = @entityId;";

            var archivedEntity = await connection.QuerySingleOrDefaultAsync<ArchivedEntity>(sql, new { entityType = @entityType, entityId = @entityId });

            await connection.CloseAsync();
            return archivedEntity;
        }

        public async Task<IEnumerable<Tuple<Vacancy, ArchivedEntity, ArchivedEntity>>> GetArchivedVacanciesAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();

            await connection.OpenAsync();

            var sql = @"SELECT V.Id, V.Title, V.CreationDate, V.CompletionDate, 
                        AV.*, U1.*, UR1.*, R1.*, 
                        P.Id, P.Name, 
                        U2.*, UR2.*, R2.*, AP.Id
                        FROM Vacancies V 
                        INNER JOIN ArchivedEntities AV ON AV.EntityType = @entityVacancyType AND AV.EntityId = V.Id  
                        INNER JOIN Users U1 ON U1.Id = AV.UserId 
                        INNER JOIN UserToRoles UR1 ON UR1.UserId = U1.Id 
                        INNER JOIN Roles R1 ON R1.Id = UR1.RoleId 
                        INNER JOIN Projects P ON P.Id = V.ProjectId 
                        INNER JOIN Users U2 ON U2.Id = V.RespONsibleHrId 
                        INNER JOIN UserToRoles UR2 ON UR2.UserId = U2.Id 
                        INNER JOIN Roles R2 ON R2.Id = UR2.RoleId 
                        LEFT JOIN  ArchivedEntities AP ON AP.EntityType = @entityProjectType AND AP.EntityId = P.Id
                        WHERE V.CompanyId = @companyId
                        ORDER BY AV.ExpirationDate DESC;";

            var vacancyDictionary = new Dictionary<string, Tuple<Vacancy, ArchivedEntity, ArchivedEntity>>();

            await connection.QueryAsync<Tuple<Vacancy, ArchivedEntity, ArchivedEntity>>(
                sql,
                new Type[] { typeof(Vacancy), typeof(ArchivedEntity), typeof(User), 
                             typeof(UserToRole), typeof(Role), typeof(Project), typeof(User), 
                             typeof(UserToRole), typeof(Role), typeof(ArchivedEntity)},
                obj =>
                {
                    Vacancy vacancy = obj[0] as Vacancy;
                    ArchivedEntity archivedVacancy = obj[1] as ArchivedEntity;
                    User archivedByHr = obj[2] as User;
                    UserToRole userToRoleArchivedByHr = obj[3] as UserToRole;
                    Role roleArchivedByHr = obj[4] as Role;
                    Project project = obj[5] as Project;
                    User responsibleHr = obj[6] as User;
                    UserToRole userToRoleResponsibleHr = obj[7] as UserToRole;
                    Role roleResponsibleHr = obj[8] as Role;
                    ArchivedEntity archivedProject = obj[9] as ArchivedEntity;

                    if (!vacancyDictionary.TryGetValue(vacancy.Id, out var vacancyEntry))
                    {
                        vacancyEntry = new Tuple<Vacancy, ArchivedEntity, ArchivedEntity>(vacancy, archivedVacancy, archivedProject); ;

                        vacancyEntry.Item1.Project = project;
                        vacancyEntry.Item1.ResponsibleHr = responsibleHr;
                        vacancyEntry.Item1.ResponsibleHr.UserRoles = new LinkedList<UserToRole>();
                        vacancyEntry.Item2.User = archivedByHr;
                        vacancyEntry.Item2.User.UserRoles = new LinkedList<UserToRole>();
                        vacancyDictionary.Add(vacancyEntry.Item1.Id, vacancyEntry);
                    }

                    if (!vacancyEntry.Item1.ResponsibleHr.UserRoles.Any(x => x.RoleId == userToRoleResponsibleHr.RoleId && x.UserId == userToRoleResponsibleHr.UserId))
                    {
                        vacancyEntry.Item1.ResponsibleHr.UserRoles.Add(userToRoleResponsibleHr);

                        if (roleResponsibleHr != null)
                        {
                            userToRoleResponsibleHr.Role = roleResponsibleHr;
                        }
                    }
                   
                    if (!vacancyEntry.Item2.User.UserRoles.Any(x => x.RoleId == userToRoleArchivedByHr.RoleId && x.UserId == userToRoleArchivedByHr.UserId))
                    {
                        vacancyEntry.Item2.User.UserRoles.Add(userToRoleArchivedByHr);

                        if (roleArchivedByHr != null)
                        {
                            userToRoleArchivedByHr.Role = roleArchivedByHr;
                        }
                    }
                      
                    return vacancyEntry;
                },
                new
                {
                    companyId = @companyId,
                    entityProjectType = EntityType.Project,
                    entityVacancyType = EntityType.Vacancy
                },
                splitOn: "Id,Id,Id,Id,Id,Id,Id,Id,Id,Id");


            await connection.CloseAsync();

            return vacancyDictionary.Values.ToList();
        }

        public async Task<IEnumerable<Tuple<Project, ArchivedEntity>>> GetArchivedProjectsAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();

            string sql = @"SELECT P.Id, P.Logo, P.Name, P.Description, P.TeamInfo, P.CreationDate, 
                           AP.*, U.*, UR.*, R.*, 
                           V.Id, V.Title, V.Description, V.Requirements
                           FROM Projects P
                           INNER JOIN ArchivedEntities AP ON AP.EntityType = @entityProjectType AND AP.EntityId = P.Id  
                           INNER JOIN Users U ON U.Id = AP.UserId 
                           INNER JOIN UserToRoles UR ON UR.UserId = U.Id 
                           INNER JOIN Roles R ON R.Id = UR.RoleId 
                           LEFT JOIN Vacancies V ON P.Id = V.ProjectId     
                           WHERE P.CompanyId = @companyId
                           ORDER BY AP.ExpirationDate DESC;";

            var projectDictionary = new Dictionary<string, Tuple<Project, ArchivedEntity>>();

            await connection.QueryAsync<Project, ArchivedEntity, User, UserToRole, Role, Vacancy, Tuple<Project, ArchivedEntity>>(
                sql, 
                (project, archivedProject, archivedByHr, userToRole, role, vacancy) =>
                {
                    if (!projectDictionary.TryGetValue(project.Id, out var projectEntry))
                    {
                        projectEntry = new Tuple<Project, ArchivedEntity>(project, archivedProject);

                        projectEntry.Item1.Vacancies = new LinkedList<Vacancy>();
                        projectEntry.Item2.User = archivedByHr;
                        projectEntry.Item2.User.UserRoles = new LinkedList<UserToRole>();
                        projectDictionary.Add(projectEntry.Item1.Id, projectEntry);
                    }

                    if (vacancy is not null)
                    {
                        if (!projectEntry.Item1.Vacancies.Any(p => p.Id == vacancy.Id))
                        {
                            projectEntry.Item1.Vacancies.Add(vacancy);
                        }
                    }
                   
                    if (!projectEntry.Item2.User.UserRoles.Any(x => x.RoleId == userToRole.RoleId && x.UserId == userToRole.UserId))
                    {
                        projectEntry.Item2.User.UserRoles.Add(userToRole);

                        if (role != null)
                        {
                            userToRole.Role = role;
                        }
                    }
                                      
                    return projectEntry;
                }, 
                new
                {
                    companyId = @companyId,
                    entityProjectType = EntityType.Project,
                    entityVacancyType = EntityType.Vacancy
                },
                splitOn: "Id,Id,Id,Id,Id,Id");

            var projects = projectDictionary.Values.ToList();

            await connection.CloseAsync();

            return projects;
        }
    }
}
