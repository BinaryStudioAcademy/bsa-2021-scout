using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IImageReadRepository
    {
        Task<string> GetPublicUrlAsync(string applicantId);
    }
}
