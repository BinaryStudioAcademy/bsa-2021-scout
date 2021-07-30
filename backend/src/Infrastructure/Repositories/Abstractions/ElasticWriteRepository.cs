﻿using Domain.Common;
using Domain.Interfaces;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using System.Collections.Generic;

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
        public async Task InsertBulk(IEnumerable<T> bulk)
        {
            await _client.IndexManyAsync<T>(bulk);
        }

        public async Task<Entity> CreateAsync(T entity)
        {
            var indexResponse = await _client.IndexDocumentAsync<T>(entity);
            if (!indexResponse.IsValid)
                throw new InvalidOperationException("Index response is invalid.");
            return entity;
        }

        public async Task<Entity> UpdateAsync(T entity)
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
    }
}
