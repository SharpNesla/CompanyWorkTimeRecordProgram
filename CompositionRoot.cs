using employees;
using employees.Elements;
using employees.Model;

namespace Employees
{
    class CompositionRoot
    {
        private ApplicationContext _applicationContext = new ApplicationContext();
        private EmployeeService employeeService => new EmployeeService(_applicationContext);
        private CardService cardService => new CardService(_applicationContext);
        private HubViewModel _hub => new HubViewModel(this.EmployeeDictionary, this.CardDictionary);
        private PaginatorViewModel paginator => new PaginatorViewModel();
        
        public EmployeeDictionaryViewModel EmployeeDictionary =>
            new EmployeeDictionaryViewModel(employeeService, paginator);

        public EmployeeEditorViewModel EmployeeEditor => new EmployeeEditorViewModel(employeeService);
        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel();
        public CardDictionaryViewModel CardDictionary => new CardDictionaryViewModel(paginator);
        public CardEditorViewModel CardEditor => new CardEditorViewModel();
        public CardInfoViewModel CardInfo => new CardInfoViewModel();
        public AuthViewModel Auth => new AuthViewModel(employeeService);
        public HubViewModel Hub => _hub;
        public ChartViewModel Chart => new ChartViewModel();
    }
}