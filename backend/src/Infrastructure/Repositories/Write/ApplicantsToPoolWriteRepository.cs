using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Write;
using Infrastructure.EF;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Linq;

namespace Infrastructure.Repositories.Write
{
    public class PoolToApplicantWriteRepository : WriteRepository<PoolToApplicant>, IPoolToApplicantWriteRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public PoolToApplicantWriteRepository(
            ApplicationDbContext context,
            IConnectionFactory connectionFactory
        ) : base(context)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task UpdatePoolApplicants(string[] newIds, string poolId, string name, string description)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = $@"SELECT Id,PoolId,ApplicantId
                            FROM PoolToApplicants
                            WHERE poolId = @id";

            var poolApplicantsfromDB = await connection.QueryAsync<PoolToApplicant>(sql, new {@id = poolId});

            sql = $@"SELECT * FROM Pools WHERE id = @id";

            var pool = await connection.QueryFirstAsync<Pool>(sql, new { @id = poolId });

            await connection.CloseAsync();

            pool.Name = name;
            pool.Description = description;
            pool.PoolApplicants = (System.Collections.Generic.ICollection<PoolToApplicant>)poolApplicantsfromDB;


            foreach (var applicant in pool.PoolApplicants.Where(at => newIds.Contains(at.ApplicantId) == false).ToList())
            {
                _context.Remove(applicant);
            }

            foreach (var id in newIds.Except(pool.PoolApplicants.Select(x=>x.ApplicantId)))
            {
                _context.Add(new PoolToApplicant() { ApplicantId = id, PoolId = poolId });
            }           

            _context.Update(pool);

            await _context.SaveChangesAsync();

        }

        public async Task DeletePoolToApplicants(string poolId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            string sql = $@"SELECT Id,PoolId,ApplicantId
                            FROM PoolToApplicants
                            WHERE poolId = @id";

            var poolApplicantsfromDB = await connection.QueryAsync<PoolToApplicant>(sql, new { @id = poolId });

            await connection.CloseAsync();
            
            foreach (var applicant in poolApplicantsfromDB)
            {
                _context.Remove(applicant);
            }

            await _context.SaveChangesAsync();

        }
    }
}
