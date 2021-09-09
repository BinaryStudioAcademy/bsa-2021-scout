using Domain.Common;
using Domain.Interfaces.Abstractions;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Abstractions
{
    public class WriteRepository<T> : IWriteRepository<T> where T : Entity
    {
        protected readonly ApplicationDbContext _context;

        public WriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public virtual async Task DeleteAsync(string id)

        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(_ => _.Id == id);

            if (entity != null)
                _context.Remove(entity);

            await _context.SaveChangesAsync();
        }
    }
}
