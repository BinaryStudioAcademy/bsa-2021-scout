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
                            SELECT p.*, pa.*, a.*, fi.*, u.*, c.*
                            FROM Pools p left outer join 
                            PoolToApplicants pa ON pa.PoolId = p.Id left outer join
                            Applicants a on a.Id = pa.ApplicantId left join
                            FileInfos fi on a.PhotoFileInfoId = fi.Id inner join
                            Companies c on c.Id = p.CompanyId inner join
                            Users u on u.Id = p.CreatedById
                            where p.id = @id
                            order by a.firstName,a.lastName";

            var poolDictionary = new Dictionary<string, Pool>();
            var poolToApplicantDictionary = new Dictionary<string, PoolToApplicant>();
            Pool cachedPool = null;

            var pool = (await connection.QueryAsync<Pool, PoolToApplicant, Applicant, FileInfo, User, Company, Pool>(
                sql,
                (pool, poolToApplicant, applicant, photo, User, Company) =>
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
                        poolEntry.Company = Company;
                        poolEntry.CreatedBy = User;
                        poolDictionary.Add(poolEntry.Id, poolEntry);
                    }

                    if (poolToApplicant != null && !poolToApplicantDictionary.TryGetValue(poolToApplicant.Id, out PoolToApplicant poolToApplicantEntry))
                    {
                        poolToApplicantEntry = poolToApplicant;
                        cachedPool.PoolApplicants.Add(poolToApplicantEntry);
                        poolToApplicantDictionary.Add(poolToApplicant.Id, poolToApplicantEntry);
                    }

                    if (applicant != null)
                    {
                        poolToApplicant.Applicant = applicant;
                        poolToApplicant.Applicant.PhotoFileInfo = photo;
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

        public async Task<List<Pool>> GetPoolsWithApplicantsAsync(string companyId)
        {
            var connection = _connectionFactory.GetSqlConnection();

            await connection.OpenAsync();
            string sql = $@"
                            SELECT p.*, pa.*, a.*,u.*,c.*
                            FROM Pools p left outer join 
                            PoolToApplicants pa ON pa.PoolId = p.Id left outer join
                            Applicants a on a.Id = pa.ApplicantId inner join
                            Companies c on c.Id = p.CompanyId inner join
                            Users u on u.Id = p.CreatedById
                            where c.Id = @companyId";
                            

            var poolDictionary = new Dictionary<string, Pool>();
            var poolToApplicantDictionary = new Dictionary<string, PoolToApplicant>();
            List<Pool> allPools = new List<Pool>();

            var pool = (await connection.QueryAsync<Pool, PoolToApplicant, Applicant, User, Company, Pool>(
                sql,
                (pool, poolToApplicant, applicant, User, Company) =>
                {

                    if (!poolDictionary.TryGetValue(pool.Id, out Pool poolEntry))
                    {

                        poolEntry = pool;
                        poolEntry.PoolApplicants = new List<PoolToApplicant>();
                        poolEntry.Company = Company;
                        poolEntry.CreatedBy = User;

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
                },
                new { @companyId = companyId }
                )); ;

            await connection.CloseAsync();

            return poolDictionary.Values.ToList();
        }


    }
}
