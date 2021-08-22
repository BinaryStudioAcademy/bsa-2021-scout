using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Write
{
    public class CandidateReviewWriteRepository : WriteRepository<CandidateReview>
    {
        private readonly IConnectionFactory _connectionFactory;
        public CandidateReviewWriteRepository(ApplicationDbContext context, IConnectionFactory connectionFactory) : base(context)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
