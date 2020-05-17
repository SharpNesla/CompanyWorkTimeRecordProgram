using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    public class EmployeeFilterDefinition
    {
        public bool IsByGender { get; set; }
        public bool IsByDateBirth { get; set; }
        public bool IsBySumWorkTime { get; set; }
        public bool IsByRole { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateBirthLowBound { get; set; }
        public DateTime DateBirthHighBound { get; set; }
        public int SumWorkTimeLowBound { get; set; }
        public int SumWorkTimeHighBound { get; set; }
        public Role Role { get; set; }
    }
}
