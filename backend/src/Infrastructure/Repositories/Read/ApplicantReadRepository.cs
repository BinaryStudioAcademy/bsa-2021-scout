using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantReadRepository : ReadRepository<Applicant>, IApplicantReadRepository
    {
        public ApplicantReadRepository(IConnectionFactory connectionFactory) : base("Applicants", connectionFactory) { }

    }
}
