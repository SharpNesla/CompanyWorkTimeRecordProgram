using employees;
using employees.Elements;
using employees.Model;

namespace Employees
{
    /// <summary>
    /// Класс, отвечающий за управление зависимостями в программе.
    /// Так же используется как View Locator для ViewModel'ей при создании View.
    /// </summary>
    class CompositionRoot
    {
        private ApplicationContext _applicationContext = new ApplicationContext();
        private EmployeeService employeeService => new EmployeeService(_applicationContext);
        private CardService cardService => new CardService(_applicationContext);
        private HubViewModel _hub => new HubViewModel(this.EmployeeDictionary, this.CardDictionary);
        private PaginatorViewModel paginator => new PaginatorViewModel();
        private ShellWindowViewModel _shell = new ShellWindowViewModel();
        
        public ShellWindowViewModel ShellWindowViewModel => _shell;

        public EmployeeDictionaryViewModel EmployeeDictionary =>
            new EmployeeDictionaryViewModel(ShellWindowViewModel, employeeService, paginator);

        public EmployeeEditorViewModel EmployeeEditor =>
            new EmployeeEditorViewModel(ShellWindowViewModel, employeeService);

        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel(ShellWindowViewModel, employeeService);

        public CardDictionaryViewModel CardDictionary =>
            new CardDictionaryViewModel(ShellWindowViewModel, cardService, paginator);

        public CardEditorViewModel CardEditor => new CardEditorViewModel(ShellWindowViewModel, cardService, employeeService);
        public CardInfoViewModel CardInfo => new CardInfoViewModel(ShellWindowViewModel, cardService);
        public AuthViewModel Auth => new AuthViewModel(employeeService);
        public DeleteDialogViewModel DeleteDialog => new DeleteDialogViewModel(ShellWindowViewModel);

        public ConnectionLostDialogViewModel ConnectionLostDialog => new ConnectionLostDialogViewModel(ShellWindowViewModel);
        public HubViewModel Hub => _hub;
        public ChartViewModel Chart => new ChartViewModel(cardService);
    }
}