using Application.Common.Exceptions;
using AutoMapper;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class VacancyTableReadRepository : ReadRepository<VacancyTable>, IVacancyTableReadRepository
    {
        private readonly IMapper _mapper;
        public VacancyTableReadRepository(IConnectionFactory connectionFactory, IMapper mapper) : base("Users", connectionFactory) 
        { 
            _mapper = mapper;
        }

        public async Task<IEnumerable<VacancyTable>> GetVacancyTablesByCompanyIdAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();

            await connection.OpenAsync();

            var sql = 
            @"
            SELECT distinct v.*,p.*,u.*,ur.*,r.*, CandidateCount.count
            FROM 
                Vacancies as v left outer join
                Projects p on p.Id = v.ProjectId inner join
                Users U on u.Id = v.ResponsibleHrId inner join
                UserToRoles ur on ur.UserId = u.Id inner join
                Roles r on r.Id = ur.RoleId
            cross apply
            (
                SELECT COUNT(*) as count FROM VacancyCandidates vc
                WHERE EXISTS(SELECT * FROM Stages WHERE Stages.VacancyId = v.Id
                AND EXISTS(SELECT * FROM CandidateToStages
                WHERE CandidateToStages.CandidateId = vc.Id
                AND CandidateToStages.StageId = Stages.Id AND CandidateToStages.DateRemoved IS NULL))
            ) as CandidateCount
            WHERE 
            v.CompanyId = @id AND p.IsDeleted = 0";

            var vacancyDictionary = new Dictionary<string, VacancyTable>();
            var userToRolesDictionary = new Dictionary<string, UserToRole>();

            var vacancy = (await connection.QueryAsync<Vacancy, Project, User, UserToRole, Role, int, VacancyTable>(
                sql,
                (vacancy, project, user, userroles, role, vacancyCount) =>
                {

                    if (!vacancyDictionary.TryGetValue(vacancy.Id, out VacancyTable vacancyEntry))
                    {
                        vacancyEntry = _mapper.Map<VacancyTable>(vacancy);

                        vacancyEntry.Project = project;
                        vacancyEntry.ResponsibleHr = user;
                        vacancyEntry.ResponsibleHr.UserRoles = new LinkedList<UserToRole>();
                        vacancyEntry.CandidatesAmount = vacancyCount;
                        vacancyDictionary.Add(vacancyEntry.Id, vacancyEntry);
                    }

                    if(!vacancyEntry.ResponsibleHr.UserRoles.Any(x=> x.RoleId == userroles.RoleId && x.UserId == userroles.UserId))
                        vacancyEntry.ResponsibleHr.UserRoles.Add(userroles);
                    

                    if (role != null)
                    {
                        userroles.Role = role;
                    }

                    return vacancyEntry;
                },
                new
                {
                    id = companyId
                },
                    splitOn: "Id,Id,Id,Id,Id,Id,count"
                ));

            await connection.CloseAsync();
            
            var res = vacancyDictionary.Values.ToList();

            return res;
        }

        public async Task<IEnumerable<VacancyTable>> GetVacancyTablesByCompanyIdAsync1(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder vacancySql = new StringBuilder();
            vacancySql.Append("SELECT VC.* FROM Vacancies as VC");
            vacancySql.Append(" JOIN Projects ON Projects.Id = VC.ProjectId");
            vacancySql.Append(" WHERE VC.CompanyId = @CompanyId AND Projects.IsDeleted='0'");
            StringBuilder candidateCountSql = new StringBuilder();
            candidateCountSql.Append("SELECT COUNT(*) FROM VacancyCandidates");
            candidateCountSql.Append(" WHERE EXISTS(SELECT * FROM Stages WHERE Stages.VacancyId = @vacancyId");
            candidateCountSql.Append(" AND EXISTS(SELECT * FROM CandidateToStages");
            candidateCountSql.Append(" WHERE CandidateToStages.CandidateId = VacancyCandidates.Id ");
            candidateCountSql.Append(" AND CandidateToStages.StageId = Stages.Id AND CandidateToStages.DateRemoved IS NULL))");
            IEnumerable<VacancyTable> vacancyTables = await connection
            .QueryAsync<VacancyTable>(vacancySql.ToString(), 
                param: new 
                {
                    CompanyId = companyId
                }
            );
            foreach (var vacancyTable in vacancyTables){
                vacancyTable.CandidatesAmount = (await connection.QueryAsync<int>(candidateCountSql.ToString(), 
                    param: new 
                    {
                        vacancyId = vacancyTable.Id
                    }
                )).First();
                string hrSql = "SELECT * FROM Users WHERE Users.Id = @responsibleId";
                vacancyTable.ResponsibleHr = (await connection.QueryAsync<User>(hrSql, param: new {
                    responsibleId = vacancyTable.ResponsibleHrId
                })).FirstOrDefault();
                string projectSql = "SELECT * FROM Projects WHERE Projects.Id = @projectId";
                vacancyTable.Project = (await connection.QueryAsync<Project>(projectSql, param: new {
                    projectId = vacancyTable.ProjectId
                })).FirstOrDefault();
            }
            // 
            await connection.CloseAsync();
            return vacancyTables;
        }

        public async Task LoadRolesAsync(User user)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM UserToRoles AS UR");
            sql.Append(" INNER JOIN Roles AS R ON UR.RoleId = R.Id");
            sql.Append(" WHERE UR.UserId = @userId;");

            var userRoles = await connection.QueryAsync<UserToRole, Role, UserToRole>(sql.ToString(),
            (ur, r) =>
            {
                ur.Role = r;
                return ur;
            },
            new { userId = user.Id });
            await connection.CloseAsync();
            user.UserRoles = userRoles as ICollection<UserToRole>;
        }
    }
}
