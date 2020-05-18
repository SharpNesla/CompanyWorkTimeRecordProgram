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
using Employees;
using employees.Model;
using Employees.Model;
using employees.ViewModelBases;

namespace employees
{
    public class EmployeeInfoViewModel : ViewModelBase
    {
        public Employee Entity { get; set; }
        public string InfoTitle => $"Работник №{Entity.Id}";

        public EmployeeInfoViewModel(IShell shell, EmployeeService service)
        {
            this.Entity = service.GetById((int)shell.LastNavigatedDialogParameter);
        }
    }
}