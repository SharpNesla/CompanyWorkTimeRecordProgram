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
using employees;

namespace Employees
{
    /// <summary>
    /// Interaction logic for Auth.xaml
    /// </summary>
    public class AuthViewModel
    { 
        public string Username { get; set; }
        private string _password;
        public ICommand TryAuthCommand { get; set; }

        public ICommand ChangePassword => new RelayCommand<PasswordBox>(x=>_password = x.Password);

        public AuthViewModel(IShell shell, EmployeeService employeeService)
        {
            this.TryAuthCommand = new RelayCommand(() =>
            {
                try
                {
                    //employeeService.Auth(Username, _password);
                    shell.NavigateByUri(CompanyUris.Hub);
                }
                catch (UnauthorizedAccessException)
                {
                    var messageQueue = shell.MessageQueue;
                    var message = "Неправильное имя пользователя или пароль";

                    messageQueue.Enqueue(message);
                }
            });
        }
    }
}
