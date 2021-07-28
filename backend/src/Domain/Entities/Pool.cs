using Domain.Enums;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Pool : Entity
    {
        public PoolType Type { get; set; }
        public string CompanyId { get; set; }

        public Company Company { get; private set; }
        public ICollection<PoolToApplicant> PoolApplicants { get; private set; }
    }
}