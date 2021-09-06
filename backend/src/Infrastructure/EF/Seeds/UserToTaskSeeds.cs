using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class UserToTaskSeeds
    {
        public static IEnumerable<UserToTask> UserToTasks { get; } = new List<UserToTask>
        {
          new UserToTask
          {
              
              ToDoTaskId = "23f7b492-983d-512a-872e-4b2d42f156ba",
              UserId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
          },
          new UserToTask
          {
              
              ToDoTaskId = "23f7b492-983d-512a-872e-4b2d42f156ba",
              UserId = "1",
          },
          new UserToTask
          {
              
              ToDoTaskId = "15c616ce-669b-5d10-9612-8d7ed29666f5",
              UserId = "2",

          },
          new UserToTask
          {
              ToDoTaskId = "15c616ce-669b-5d10-9612-8d7ed29666f5",
              UserId = "1",

          },
          new UserToTask
          {
              ToDoTaskId = "15c616ce-669b-5d10-9612-8d7ed29666f5",
              UserId = "fb055f22-c5d1-4611-8e49-32a46edf58b2",

          },
          new UserToTask
          {
              ToDoTaskId = "0c32a3b2-80e9-5143-91bc-179e7df275d7",
              UserId = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
          },
          new UserToTask
          {
              ToDoTaskId = "0c32a3b2-80e9-5143-91bc-179e7df275d7",
              UserId = "1",

          },
          new UserToTask
          {
              ToDoTaskId = "0c32a3b2-80e9-5143-91bc-179e7df275d7",
              UserId = "2",
              

          },
          new UserToTask
          {
              ToDoTaskId = "44e8870c-afda-5328-8b82-64be589af4a0",
              UserId = "fb055f22-c5d1-4611-8e49-32a46edf58b2",

          },
          new UserToTask
          {
              ToDoTaskId = "44e8870c-afda-5328-8b82-64be589af4a0",  
              UserId = "057c23ff-58b1-4531-8012-0b5c1f949ee1", 

          },
          new UserToTask
          {
              ToDoTaskId = "44e8870c-afda-5328-8b82-64be589af4a0",
              UserId = "1",

          },
          new UserToTask
          {
              ToDoTaskId = "a661cd02-c014-5c13-8fc3-99febd62d470",
              UserId = "1",

          },
          new UserToTask
          {              
              ToDoTaskId = "a661cd02-c014-5c13-8fc3-99febd62d470",
              UserId = "2",

          },
           new UserToTask
          {
              ToDoTaskId = "a661cd02-c014-5c13-8fc3-99febd62d469",
              UserId = "2",

          },
          new UserToTask
          {
              ToDoTaskId = "a661cd02-c014-5c13-8fc3-99febd62d469",
              UserId = "1",

          },


        };
    }
}