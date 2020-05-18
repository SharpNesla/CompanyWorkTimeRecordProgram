using Employees;
using employees.Model;
using Employees.Model;

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