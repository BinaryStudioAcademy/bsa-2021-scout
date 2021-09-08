using System;
using Domain.Common;

namespace Domain.Entities.Abstractions
{
    public abstract class Human : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}