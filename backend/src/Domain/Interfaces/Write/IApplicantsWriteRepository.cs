using Domain.Common;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Write
{
    public interface IApplicantsWriteRepository : IWriteRepository<Applicant>
    {
        Task<Applicant> CreateFullAsync(Applicant entity);
    }
}
