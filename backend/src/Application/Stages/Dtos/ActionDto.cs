﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Enums;

namespace Application.Stages.Dtos
{
    public class ActionDto : Dto
    {
        public string Name { get; set; }
        public ActionType ActionType { get; set; }
        public string StageId { get; set; }
    }
}