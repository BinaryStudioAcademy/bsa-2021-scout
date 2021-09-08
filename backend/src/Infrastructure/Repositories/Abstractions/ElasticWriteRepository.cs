using Domain.Common;
using Domain.Interfaces.Abstractions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Nest;

namespace Infrastructure.Repositories.Abstractions
{
    public class ElasticWriteRepository<T> : IElasticWriteRepository<T> where T : Entity
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;
        public ElasticWriteRepository(IElasticClient client)
        {
            _indexName = typeof(T).ToString();
            _client = client;
        }
        public async Task InsertBulkAsync(IEnumerable<T> bulk)
        {
            await _client.IndexManyAsync<T>(bulk);
        }

        public async Task<T> CreateAsync(T entity)
        {
            var indexResponse = await _client.IndexDocumentAsync<T>(entity);
            if (!indexResponse.IsValid)
                throw new InvalidOperationException("Index response is invalid.");
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            var indexResponse = await _client.UpdateAsync<T>(
                DocumentPath<T>.Id(entity.Id),
                i => i.Doc(entity));
            return entity;
        }
        public async Task DeleteAsync(string id)
        {
            await _client.DeleteAsync<T>(id);
        }

        public async Task UpdateAsyncPartially(string id, ExpandoObject dynamicUpdate)
        {
            var indexResponse = await _client.UpdateAsync<T, object>(
                DocumentPath<T>.Id(id),
                i => i.Doc(dynamicUpdate));

            if (!indexResponse.IsValid)
                throw new InvalidOperationException("Partial update invalid");
        }
    }
}
