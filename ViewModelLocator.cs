using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using employees;
using employees.Elements;
using employees.Model;

namespace Employees
{
    class ViewModelLocator
    {
        private ApplicationContext _applicationContext = new ApplicationContext();
        private EmployeeService employeeService => new EmployeeService(_applicationContext);
        private CardService cardService => new CardService();
        private PaginatorViewModel paginator => new PaginatorViewModel();
        public EmployeeDictionaryViewModel EmployeeDictionary => new EmployeeDictionaryViewModel(paginator);
        public EmployeeEditorViewModel EmployeeEditor => new EmployeeEditorViewModel();
        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel();
        public CardDictionaryViewModel CardDictionary => new CardDictionaryViewModel();
        public AuthViewModel Auth => new AuthViewModel(employeeService);
        public CardEditorViewModel CardEditor => new CardEditorViewModel();
        public HubViewModel Hub => new HubViewModel();
    }
}
