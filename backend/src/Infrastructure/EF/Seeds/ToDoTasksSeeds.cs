using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class ToDoTasksSeeds
    {
        public static IEnumerable<ToDoTask> Tasks { get; } = new List<ToDoTask>
        {
          new ToDoTask{
              Id = "23f7b492-983d-512a-872e-4b2d42f156ba",
              Name = "Interview",
              DateCreated = new DateTime(2021, 4, 13),
              DueDate = new DateTime(2021, 4, 13).AddDays(3),
              DoneDate = new DateTime(2021, 4, 13).AddDays(3),
              IsDone = true,
              ApplicantId = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
              CreatedById = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
              Note = "This extra valuable candidate will has great interview.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
          new ToDoTask{
              Id = "15c616ce-669b-5d10-9612-8d7ed29666f5",
              Name = "Review candidates",
              DateCreated = new DateTime(2021, 5, 14),
              DueDate = new DateTime(2021, 5, 14).AddDays(3),
              DoneDate = new DateTime(2021, 5, 14).AddDays(3),
              IsDone = true,
              ApplicantId = "342f8205-6c3f-5b33-8148-bf6fa9ebda64",
              CreatedById = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
              Note = "Some candidates need our review.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
          new ToDoTask{
              Id = "0c32a3b2-80e9-5143-91bc-179e7df275d7",
              Name = "Interview",
              DateCreated = new DateTime(2021, 8, 25),
              DueDate = new DateTime(2021, 09, 01),
              IsDone = false,
              ApplicantId = "6a9e4cf9-1a02-50dd-9d32-d82fa566c5a2",
              CreatedById = "2",
              Note = "Candidate has enough work experience, however there is a big question about our posibilities to pay him enouth.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
          new ToDoTask{
              Id = "44e8870c-afda-5328-8b82-64be589af4a0",
              Name = "Test english skill",
              DateCreated = new DateTime(2021, 8, 27),
              DueDate = new DateTime(2021, 9, 5),
              IsDone = false,
              ApplicantId = "98954928-1256-557a-b79f-7058641f0b4b",
              CreatedById = "f8afcbaa-54bd-4103-b3f0-0dd17a5226ca",
              Note = "Just another simple speaking test.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
          new ToDoTask{
              Id = "a661cd02-c014-5c13-8fc3-99febd62d469",
              Name = "Skype tech interview",
              DateCreated = new DateTime(2021, 8, 30),
              DueDate = new DateTime(2021, 9,5),
              IsDone = false,
              ApplicantId = "d6f8a112-c9bb-5e0e-a960-53a9da2495ab",
              CreatedById = "8be08dba-5dac-408a-ab6e-8d162af74024",
              Note = "Candidate was working for the current company, than leaved and now reappling again. Wanna watch how is he looking after that.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
          new ToDoTask{
              Id = "a661cd02-c014-5c13-8fc3-99febd62d470",
              Name = "Live coding session",
              DateCreated = new DateTime(2021, 8, 13),
              DueDate = new DateTime(2021, 8, 13).AddDays(3),
              DoneDate = new DateTime(2021, 8, 13).AddDays(3),
              IsDone = true,
              ApplicantId = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
              CreatedById = "2",
              Note = "I like to move it, move it",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
              IsReviewed = true
          },
        };
    }
}