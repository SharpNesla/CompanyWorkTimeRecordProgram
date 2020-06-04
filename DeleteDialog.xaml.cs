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

namespace employees
{
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику удаления сущности
    /// </summary>
    public class DeleteDialogViewModel
    {
        private readonly IShell _shell;

        public ICommand ApplyCommand =>
            new RelayCommand(() =>
            {
                var parameters = _shell.LastNavigatedDialogParameter as object[];
                var id = (int) parameters[0];
                try
                {
                    if (parameters[1] is EmployeeService)
                    {
                        (parameters[1] as EmployeeService).Remove(id);
                    }
                    else
                    {
                        (parameters[1] as CardService).Remove(id);
                    }

                    _shell.CloseDialog();
                }
                catch (Exception)
                {
                    _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                }
            });

        public ICommand RejectCommand => new RelayCommand(() => _shell.CloseDialog());

        public DeleteDialogViewModel(IShell shell)
        {
            _shell = shell;
        }
    }
}