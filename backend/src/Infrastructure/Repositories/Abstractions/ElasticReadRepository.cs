﻿using Domain.Common;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;
using Nest;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IReadOnlyCollection<T>> SearchByQuery(string querty)
        {
            var searchResponse = await _client.SearchAsync<T>(s => s.QueryOnQueryString(querty));
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
    }
}
