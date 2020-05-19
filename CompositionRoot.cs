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
        private PaginatorViewModel paginator => new PaginatorViewModel();
        public ShellWindowViewModel ShellWindowViewModel { get; } = new ShellWindowViewModel();

        public EmployeeDictionaryViewModel EmployeeDictionary =>
            new EmployeeDictionaryViewModel(ShellWindowViewModel, employeeService, paginator);

        public EmployeeEditorViewModel EmployeeEditor =>
            new EmployeeEditorViewModel(ShellWindowViewModel, employeeService);

        public EmployeeInfoViewModel EmployeeInfo => new EmployeeInfoViewModel(ShellWindowViewModel, employeeService);

        public CardDictionaryViewModel CardDictionary =>
            new CardDictionaryViewModel(ShellWindowViewModel, employeeService, cardService, paginator);

        public CardEditorViewModel CardEditor =>
            new CardEditorViewModel(ShellWindowViewModel, cardService, employeeService);

        public CardInfoViewModel CardInfo => new CardInfoViewModel(ShellWindowViewModel, cardService);
        public AuthViewModel Auth => new AuthViewModel(ShellWindowViewModel, employeeService);
        public DeleteDialogViewModel DeleteDialog => new DeleteDialogViewModel(ShellWindowViewModel);

        public ConnectionLostDialogViewModel ConnectionLostDialog =>
            new ConnectionLostDialogViewModel(ShellWindowViewModel);
        public HubViewModel Hub => new HubViewModel(this.EmployeeDictionary, this.CardDictionary);
        public ChartViewModel Chart => new ChartViewModel(cardService);
    }
}