using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Autofac;
using employees;
using employees.Model;

namespace Employees
{
    class ViewModelLocator
    {
        private ApplicationContext _applicationContext = new ApplicationContext();
        private EmployeeService employeeService => new EmployeeService(_applicationContext);
        private CardService cardService => new CardService();

        public EmployeeDictionaryViewModel EmployeeDictionary => new EmployeeDictionaryViewModel();
        public CardDictionaryViewModel CardDictionary => new CardDictionaryViewModel();

        public AuthViewModel Auth => new AuthViewModel(employeeService);
        public CardEditorViewModel CardEditor => new CardEditorViewModel();
        public HubViewModel hubViewModel => new HubViewModel();
    }
}
