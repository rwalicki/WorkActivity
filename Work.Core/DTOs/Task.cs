﻿using Shared.Models;
using System;
using System.Collections.Generic;

namespace Work.Core.DTOs
{
    public class Task : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public List<int> SprintIds { get; set; }
    }
}