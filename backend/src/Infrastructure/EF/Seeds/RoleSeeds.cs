using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class RoleSeeds
    {
        public static IList<Role> Roles { get; } = new List<Role>
        {
            new Role 
            { 
                Id = "1", 
                Key = 1, 
                Name = "HrLead" 
            },
            new Role 
            { 
                Id = "2", 
                Key = 2, 
                Name = "HrUser"
            }
        };
    }
}