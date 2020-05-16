using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    public class WorkLoadFilterDefinition
    {
        public bool IsByGender { get; set; }
        public bool IsByDatePass { get; set; }
        public bool IsByEmployee { get; set; }
        public bool IsByDateBirth { get; set; }
    }
}
