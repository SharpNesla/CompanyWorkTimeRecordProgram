using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace employees.Model
{
    public class CardFilterDefinition
    {
        public bool IsBySumWorkTime { get; set; }
        public bool IsByDatePass { get; set; }
        public bool IsByEmployee { get; set; }
    }
}
