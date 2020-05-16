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
    class CompositionRoot
    {
        private ApplicationContext _applicationContext = new ApplicationContext();
        private EmployeeService employeeService => new EmployeeService(_applicationContext);
        private CardService cardService => new CardService();
        private HubViewModel _hub;
        private PaginatorViewModel paginator => new PaginatorViewModel();
       
        public EmployeeDictionaryViewModel EmployeeDictionary => new EmployeeDictionaryViewModel(paginator);
        
        public CompositionRoot()
        {
            _hub = new HubViewModel(this.EmployeeDictionary, this.CardDictionary);
        }

        public EmployeeEditorViewModel EmployeeEditor => new EmployeeEditorViewModel();
        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel();
        public CardDictionaryViewModel CardDictionary => new CardDictionaryViewModel(paginator);
        public CardEditorViewModel CardEditor => new CardEditorViewModel();
        public CardInfoViewModel CardInfo => new CardInfoViewModel();
        public AuthViewModel Auth => new AuthViewModel(employeeService);
        public HubViewModel Hub => _hub;
        public ChartViewModel Chart => new ChartViewModel();
    }
}
