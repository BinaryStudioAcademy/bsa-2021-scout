using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IService<TDto> where TDto: Dto
    {
        Task<TDto> CreateAsync(TDto dto);
        Task<TDto> GetAsync(Guid id);
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> UpdateAsync(TDto dto);
        Task DeleteAsync(Guid id);
    }
}
