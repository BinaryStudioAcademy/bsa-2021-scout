using Application.Common.Exceptions;
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
    public class InterviewReadRepository : ReadRepository<Interview>, IInterviewReadRepository
    {
        public InterviewReadRepository(IConnectionFactory connectionFactory) : base("Interviews", connectionFactory) { }


        public async Task<IEnumerable<Interview>> GetInterviewsByCompanyIdAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT *");
            sql.Append(" FROM Interviews");
            sql.Append($" WHERE Interviews.CompanyId = @companyId");

            IEnumerable<Interview> interviews = await connection
                .QueryAsync<Interview>(sql.ToString(), new { companyId = @companyId });
            await connection.CloseAsync();
            foreach(var interview in interviews)
            {
                await LoadCandidateAsync(interview);
            }
            return interviews;
        }
        public async Task LoadCandidateAsync(Interview interview)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            var sql = new StringBuilder();
            sql.Append("SELECT * ");
            sql.Append(" FROM Applicants");
            sql.Append(" WHERE Applicants.Id = @candidateId;");

            IEnumerable<Applicant> applicants = await connection
                .QueryAsync<Applicant>(sql.ToString(), new { candidateId = interview.CandidateId });
           
            await connection.CloseAsync();
            interview.Candidate = applicants.FirstOrDefault();
        }
        public async Task LoadInterviewersAsync(Interview interview)
        {
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            var sql = new StringBuilder();
            sql.Append($"SELECT * FROM UsersToInterviews AS UI");
            sql.Append(" INNER JOIN Users AS U ON UI.UserId = U.Id");
            sql.Append(" WHERE UI.InterviewId = @interviewId;");

            var interviewers = await connection.QueryAsync<UsersToInterview, User, UsersToInterview>(sql.ToString(),
            (ui, u) =>
            {
                ui.User = u;
                return ui;
            },
            new { interviewId = interview.Id });
            await connection.CloseAsync();
            interview.UserParticipants = interviewers as ICollection<UsersToInterview>;
        }
    }
}
