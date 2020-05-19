using System.Windows.Input;
using Employees;
using employees.Model;
using Employees.Model;

namespace employees
{
    public class EmployeeInfoViewModel : ViewModelBase
    {
        public Employee Entity { get; set; }
        public string InfoTitle => $"Работник №{Entity.Id}";
        public ICommand ViewCardInfoCommand { get; }
        public ICommand ApplyCommand { get; set; }
        public EmployeeInfoViewModel(IShell shell, EmployeeService service)
        {
            this.Entity = service.GetById((int)shell.LastNavigatedDialogParameter);
            ViewCardInfoCommand = new RelayCommand<Card>(
                x => shell.OpenDialogByUri(CompanyUris.CardInfo, true, x.Id));
            ApplyCommand = new RelayCommand(() => shell.TryCloseDialog());
        }

    }
}