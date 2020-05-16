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
using employees.Elements;
using employees.Model;
using Employees.Model;

namespace employees
{
    public class HubViewModel : ViewModelBase
    {
        public bool IsEmployeeTab { get; set; }
        public bool IsCardTab { get; set; } = true;
        public EmployeeDictionaryViewModel EmployeeDictionaryViewModel { get; }
        public CardDictionaryViewModel CardDictionaryViewModel { get; }

        public HubViewModel(EmployeeDictionaryViewModel employeeDictionaryViewModel,
            CardDictionaryViewModel cardDictionaryViewModel)
        {
            EmployeeDictionaryViewModel = employeeDictionaryViewModel;
            CardDictionaryViewModel = cardDictionaryViewModel;
        }
    }

    public class DictionaryViewModelBase<TEntity, TFilter> : ViewModelBase where TFilter : new()
    {
        public bool IsFilterDrawerOpened { get; set; } = true;
        public TFilter FilterDefinition { get; set; } = new TFilter();

        public PaginatorViewModel PaginatorViewModel { get; private set; }

        public DictionaryViewModelBase(PaginatorViewModel paginatorViewModel)
        {
            PaginatorViewModel = paginatorViewModel;
        }
    }

    public class EmployeeDictionaryViewModel : DictionaryViewModelBase<Employee, EmployeeFilterDefinition>
    {
        public List<Employee> Entities { get; set; } = new List<Employee>(new[]
            {
                new Employee
                {
                    Id = 12,
                    Name = "Василий",
                    Surname = "Иванов",
                    Patronymic = "Иванович",
                    PhoneNumber = "80572523765",
                    DateBirth = new DateTime(1985, 3, 4)
                },
                new Employee
                {
                    Id = 12,
                    Name = "Василий",
                    Surname = "Иванов",
                    Patronymic = "Иванович",
                    PhoneNumber = "80572523765",
                    DateBirth = new DateTime(1985, 3, 4)
                },
                new Employee
                {
                    Id = 12,
                    Name = "Василий",
                    Surname = "Иванов",
                    Patronymic = null,
                    PhoneNumber = "80572523765",
                    DateBirth = new DateTime(1985, 3, 4)
                },
                new Employee
                {
                    Id = 12,
                    Name = "Василий",
                    Surname = "Иванов",
                    Patronymic = "Иванович",
                    PhoneNumber = "80572523765",
                    DateBirth = new DateTime(1985, 3, 4)
                },
                new Employee
                {
                    Id = 12,
                    Name = "Василий",
                    Surname = "Иванов",
                    Patronymic = "Иванович",
                    PhoneNumber = "80572523765",
                    DateBirth = new DateTime(1985, 3, 4)
                }
            }
        );

        public EmployeeDictionaryViewModel(PaginatorViewModel paginatorViewModel) : base(paginatorViewModel)
        {
        }
    }

    public class CardDictionaryViewModel : DictionaryViewModelBase<Card, CardFilterDefinition>
    {
        public List<Card> Entities { get; set; } = new List<Card>(new[]
            {new Card {WorkLoadTimeWednesday = 843, Employee = new Employee {Name = "Василий"}}});

        public CardDictionaryViewModel(PaginatorViewModel paginatorViewModel) : base(paginatorViewModel)
        {
        }
    }
}