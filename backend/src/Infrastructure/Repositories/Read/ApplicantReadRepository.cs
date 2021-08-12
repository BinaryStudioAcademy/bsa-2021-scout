using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces.Read;
using Domain.Entities;
using Application.Common.Exceptions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantReadRepository : ReadRepository<Applicant>, IApplicantReadRepository
    {
        public ApplicantReadRepository(IConnectionFactory connectionFactory) : base("Applicants", connectionFactory) { }

        public async Task<FileInfo> GetCvFileInfoAsync(string applicantId)
        {
            SqlConnection connection = _connectionFactory.GetSqlConnection();

            var query = @"SELECT fi.* FROM Applicants a
                          INNER JOIN FileInfos fi ON a.CvFileInfoId = fi.Id
                          WHERE a.Id = @applicantId;";

            var fileInfo = await connection.QueryFirstAsync<FileInfo>(query, new { applicantId = applicantId });

            if (fileInfo == null)
            {
                throw new NotFoundException(typeof(FileInfo), applicantId);
            }

            Console.WriteLine($"{fileInfo.Id} {fileInfo.Name}");

            await connection.CloseAsync();

            return fileInfo;
        }
    }
}
