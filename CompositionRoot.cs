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
        private EmployeeService _employeeService;
        private CardService _cardService;

        public CompositionRoot()
        {
            _employeeService = new EmployeeService(_applicationContext);
            _cardService = new CardService(_applicationContext);
        }

        private PaginatorViewModel paginator => new PaginatorViewModel();
        public ShellWindowViewModel ShellWindowViewModel { get; } = new ShellWindowViewModel();

        public EmployeeDictionaryViewModel EmployeeDictionary =>
            new EmployeeDictionaryViewModel(ShellWindowViewModel, _employeeService, paginator);

        public EmployeeEditorViewModel EmployeeEditor =>
            new EmployeeEditorViewModel(ShellWindowViewModel, _employeeService);

        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel(ShellWindowViewModel, _employeeService);

        public CardDictionaryViewModel CardDictionary =>
            new CardDictionaryViewModel(ShellWindowViewModel, _employeeService, _cardService, paginator);

        public CardEditorViewModel CardEditor =>
            new CardEditorViewModel(ShellWindowViewModel, _cardService, _employeeService);

        public CardInfoViewModel CardInfo => new CardInfoViewModel(ShellWindowViewModel, _cardService);
        public AuthViewModel Auth => new AuthViewModel(ShellWindowViewModel, _employeeService);
        public DeleteDialogViewModel DeleteDialog => new DeleteDialogViewModel(ShellWindowViewModel);

        public ConnectionLostDialogViewModel ConnectionLostDialog =>
            new ConnectionLostDialogViewModel(ShellWindowViewModel);

        public HubViewModel Hub => new HubViewModel(this.EmployeeDictionary, this.CardDictionary);
        public ChartViewModel Chart => new ChartViewModel(ShellWindowViewModel, _cardService);
    }
}