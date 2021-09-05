using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Write
{
    public interface IImageWriteRepository
    {
        Task<FileInfo> UploadAsync(string applicantId, System.IO.Stream cvFileContent);
        Task UpdateAsync(string applicantId, System.IO.Stream cvFileContent);
        Task DeleteAsync(FileInfo fileInfo);
    }
}
