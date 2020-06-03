using System;
using System.Windows.Input;
using Employees;
using employees.Model;
using Employees.Model;

namespace employees
{
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику
    /// демонстрации информации о работнике
    /// </summary>
    public class EmployeeInfoViewModel : ViewModelBase
    {
        public bool IsWriteRights { get; }
        public Employee Entity { get; set; }

        #region Команды нажатий на кнопки

        public ICommand ViewCardInfoCommand { get; }
        public ICommand ApplyCommand { get; set; }
        public ICommand OpenEditor { get; }

        #endregion
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

            this.OpenEditor = new RelayCommand(() =>
            {
                shell.NavigateByUri(CompanyUris.EmployeeEditor, this.Entity.Id);
                shell.CloseDialog();
            });

            ApplyCommand = new RelayCommand(() => shell.CloseDialog());
        }

    }
}