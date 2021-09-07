using Domain.Common;
using Domain.Interfaces.Abstractions;
using System;
using System.Threading.Tasks;
using Nest;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using System.Threading;

namespace Infrastructure.Repositories.Abstractions
{
    public class ElasticReadRepository<T> : IElasticReadRepository<T> where T : Entity 
    { 
        private readonly IElasticClient _client;
        private readonly string _indexName;

        public ElasticReadRepository(IElasticClient client, string indexName = nameof(T))
        {
            _client = client;
            _indexName = indexName;
        }

        public async Task<IReadOnlyCollection<T>> SearchByQuery(string query, CancellationToken _)
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.QueryOnQueryString(query));
            if (!searchResponse.IsValid)
                throw new ArgumentException("Search query is invalid");
            return searchResponse.Documents;
        }
        public async Task<T> GetAsync(string id)
        {
            var result = await _client.GetAsync<T>(id);
            return result.Source;
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            var result = await _client.SearchAsync<T>();
            return result.Documents;
        }

        public async Task<T> GetByPropertyAsync(string property, string propertyValue)
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.QueryOnQueryString($"{property}: {propertyValue}"));
            if (!searchResponse.IsValid)
                throw new ArgumentException("Search properties is invalid");
            return searchResponse.Documents.FirstOrDefault();
        }

        public async Task<IEnumerable<T>> GetEnumerableByPropertyAsync(string property, string propertyValue)
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.QueryOnQueryString($"{property}: {propertyValue}"));
            if (!searchResponse.IsValid)
                throw new ArgumentException("Search properties is invalid");
            return searchResponse.Documents;
        }
    }
}
