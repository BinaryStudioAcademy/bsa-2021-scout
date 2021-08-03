using Domain.Common;
using System;

namespace Domain.Entities
{
    public abstract class Human: Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
    }
}