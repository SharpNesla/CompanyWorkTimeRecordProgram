using System;
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
        public bool IsWriteRights { get; }

        public EmployeeInfoViewModel(IShell shell, EmployeeService service)
        {
            try
            {
                this.Entity = service.GetById((int) shell.LastNavigatedDialogParameter);
                this.IsWriteRights = service.CurrentUser.Role == Role.Manager;
            }
            catch (Exception e)
            {
                shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                return;
            }

            ViewCardInfoCommand = new RelayCommand<Card>(
                x => shell.OpenDialogByUri(CompanyUris.CardInfo, true, x.Id));
            ApplyCommand = new RelayCommand(() => shell.TryCloseDialog());
        }

    }
}