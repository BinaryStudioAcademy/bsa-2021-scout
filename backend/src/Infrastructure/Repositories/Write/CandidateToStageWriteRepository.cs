using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Write
{
    public class CandidateToStageWriteRepository : WriteRepository<CandidateToStage>, ICandidateToStageWriteRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public CandidateToStageWriteRepository(
            ApplicationDbContext context,
            IConnectionFactory connectionFactory
        ) : base(context)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
