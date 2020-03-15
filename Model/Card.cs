using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    public class Card : Entity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public decimal Payment { get; set; }
        public DateTime DatePass { get; set; }
        public int WorkLoadTimeMonday { get; set; }
        public int WorkLoadTimeTuesday { get; set; }
        public int WorkLoadTimeWednesday { get; set; }
        public int WorkLoadTimeThursday { get; set; }
        public int WorkLoadTimeFriday { get; set; }
    }
}
