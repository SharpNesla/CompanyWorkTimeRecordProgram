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
using employees;
using employees.Model;
using Employees.Model;

namespace Employees
{
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику демонстрации информации о карточке
    /// </summary>
    public class CardInfoViewModel : ViewModelBase
    {
        public Card Entity { get; set; }
        public List<Card> CardArray => new List<Card>(new[] {this.Entity});
        #region Команды нажатий на кнопки
        public ICommand ViewEmployeeInfoCommand { get; }
        public ICommand ApplyCommand { get; }
        public ICommand OpenEditor { get; } 
        #endregion
        public bool IsWriteRights { get; }
        public CardInfoViewModel(IShell shell, CardService service, EmployeeService employeeService)
        {
            try
            {
                this.Entity = service.GetById((int) shell.LastNavigatedDialogParameter);
                this.IsWriteRights = employeeService.CurrentUser.Role == Role.Manager;
            }
            catch (Exception)
            {
                shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                return;
            }

            this.ViewEmployeeInfoCommand =
                new RelayCommand(
                    () => shell.OpenDialogByUri(CompanyUris.EmployeeInfo, true, this.Entity.EmployeeId));
            this.OpenEditor = new RelayCommand(() =>
            {
                shell.NavigateByUri(CompanyUris.CardEditor, this.Entity.Id);
                shell.CloseDialog();
            });


            this.ApplyCommand = new RelayCommand(() => shell.CloseDialog());
        }
    }
}