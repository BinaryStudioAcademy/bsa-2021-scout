using Infrastructure.Dapper.Interfaces;
using Microsoft.Data.SqlClient;
using System;

namespace Infrastructure.Dapper.Services
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public ConnectionFactory()
        {
            _connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

            if (_connectionString is null)
                throw new Exception("Database connection string is not specified");
        }

        public SqlConnection GetSqlConnection()
        {
            return _connection ??= new SqlConnection(_connectionString);
        }
    }
}
