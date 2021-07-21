using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    internal class Service<TEntity, TDto> : IService<TDto>
        where TEntity: Entity
        where TDto : Dto
    {
        private protected ApplicationDbContext _context;
        private protected IMapper _mapper;

        public Service(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            
            _context.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<TDto>(entity);
        }

        public async Task<TDto> GetAsync(Guid id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(_ => _.Id == id);

            if (entity is null)
                throw new NotFoundException(typeof(TEntity), id);

            return _mapper.Map<TDto>(entity);
        }

        public Task<IEnumerable<TDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TDto> UpdateAsync(TDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
