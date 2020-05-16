﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    public class EmployeeFilterDefinition
    {
        public bool IsByGender { get; set; }
        public bool IsByDateBirth { get; set; }
        public bool IsBySumWorkTime { get; set; }
        public bool IsByRole { get; set; }
    }
}
