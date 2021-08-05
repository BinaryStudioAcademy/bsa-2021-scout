using System;
using Domain.Common;

namespace Domain.Entities.Abstractions
{
    public abstract class Human : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
    }
}