using Domain.Common;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Nest;
using System.Collections.Generic;
using System.Linq;
namespace Infrastructure.Repositories.Abstractions
{
    public class ElasticReadRepository<T> : IReadRepository<T> where T : Entity
    {
        private readonly IElasticClient _client;
        private readonly string _indexName;
            
        public ElasticReadRepository(IElasticClient client, string indexName= nameof(T))
        {
            _client = client;
            _indexName = indexName;
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
    }
}
