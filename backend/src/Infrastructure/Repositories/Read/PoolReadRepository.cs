using Application.Pools.Dtos;
using Dapper;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class PoolReadRepository : ReadRepository<Pool>, IPoolReadRepository
    {
        public PoolReadRepository(IConnectionFactory connectionFactory) : base("Pools", connectionFactory) { }

        public async Task<Pool> GetPoolWithApplicantsByIdAsync(string id)
        {
            var connection = _connectionFactory.GetSqlConnection();

            await connection.OpenAsync();
            string sql = $@"
                            SELECT p.*, pa.*, a.*
                            FROM Pools p left outer join 
                            PoolToApplicants pa ON pa.PoolId = p.Id left outer join
                            Applicants a on a.Id = pa.ApplicantId
                            where p.id = @id";

            var poolDictionary = new Dictionary<string, Pool>();
            var poolToApplicantDictionary = new Dictionary<string, PoolToApplicant>();
            Pool cachedPool = null;

            var pool = (await connection.QueryAsync<Pool, PoolToApplicant, Applicant, Pool>(
                sql,
                (pool, poolToApplicant, applicant) =>
                {
                    if (cachedPool == null)
                    {
                        cachedPool = pool;
                        cachedPool.PoolApplicants = new List<PoolToApplicant>();
                    }

                    if (!poolDictionary.TryGetValue(pool.Id, out Pool poolEntry))
                    {
                        poolEntry = pool;
                        poolEntry.PoolApplicants = new List<PoolToApplicant>();
                        poolDictionary.Add(poolEntry.Id, poolEntry);
                    }

                    if (poolToApplicant!=null && !poolToApplicantDictionary.TryGetValue(poolToApplicant.Id, out PoolToApplicant poolToApplicantEntry))
                    {
                        poolToApplicantEntry = poolToApplicant;
                        cachedPool.PoolApplicants.Add(poolToApplicantEntry);
                        poolToApplicantDictionary.Add(poolToApplicant.Id, poolToApplicantEntry);
                    }

                    if (applicant != null)
                    {
                        poolToApplicant.Applicant = applicant;
                    }


                    return cachedPool;
                },
                new { id = @id }
                ))
            .Distinct()
            .SingleOrDefault();            

            await connection.CloseAsync();

            return pool;
        }

        public async Task<List<Pool>> GetPoolsWithApplicantsAsync()
        {
            var connection = _connectionFactory.GetSqlConnection();

            await connection.OpenAsync();
            string sql = $@"
                            SELECT p.*, pa.*, a.*
                            FROM Pools p left outer join 
                            PoolToApplicants pa ON pa.PoolId = p.Id left outer join
                            Applicants a on a.Id = pa.ApplicantId";
                            

            var poolDictionary = new Dictionary<string, Pool>();
            var poolToApplicantDictionary = new Dictionary<string, PoolToApplicant>();
            List<Pool> allPools = new List<Pool>();

            var pool = (await connection.QueryAsync<Pool, PoolToApplicant, Applicant, Pool>(
                sql,
                (pool, poolToApplicant, applicant) =>
                {
                    
                    if (!poolDictionary.TryGetValue(pool.Id, out Pool poolEntry))
                    {

                        poolEntry = pool;
                        poolEntry.PoolApplicants = new List<PoolToApplicant>();

                        poolDictionary.Add(poolEntry.Id, poolEntry);
                    }

                    if (poolToApplicant != null && !poolToApplicantDictionary.TryGetValue(poolToApplicant.Id, out PoolToApplicant poolToApplicantEntry))
                    {
                        poolToApplicantEntry = poolToApplicant;
                        poolEntry.PoolApplicants.Add(poolToApplicantEntry);
                        poolToApplicantDictionary.Add(poolToApplicant.Id, poolToApplicantEntry);
                    }

                    if (applicant != null)
                    {
                        poolToApplicant.Applicant = applicant;
                    }

                    return poolEntry;
                }
                ));

            await connection.CloseAsync();

            return poolDictionary.Values.ToList();
        }


    }
}
