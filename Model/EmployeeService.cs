using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees.Model;

namespace employees.Model
{
    public class EmployeeService
    {
        private readonly ApplicationContext _applicationContext;

        public EmployeeService(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }


    }
}
