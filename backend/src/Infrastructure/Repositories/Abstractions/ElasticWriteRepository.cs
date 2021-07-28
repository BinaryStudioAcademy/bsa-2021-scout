using Domain.Common;
using Domain.Interfaces;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace Infrastructure.Repositories.Abstractions
{
    public class ElasticWriteRepository<T> : IWriteRepository<T> where T : Entity
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;
            
        public ElasticWriteRepository( IElasticClient client, string indexName= nameof(T))
        {
            _client = client;
            _indexName = indexName;
        }

        public async Task<Entity> CreateAsync(T entity)
        {
            var indexResponse = await _client
                .IndexAsync(entity, i => i.Index(_indexName));
            if (!indexResponse.IsValid)
                throw new InvalidOperationException("Index response is invalid.");
            return entity;
        }

        public async Task<Entity> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
