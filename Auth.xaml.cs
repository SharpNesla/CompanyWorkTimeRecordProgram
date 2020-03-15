using employees.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Employees
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public class AuthViewModel
    {
        private readonly EmployeeService employeeService;

        public AuthViewModel(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }
    }
}
