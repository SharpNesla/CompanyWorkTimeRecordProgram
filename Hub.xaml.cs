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
        public bool IsEmployeeTab { get; set; } = true;
        public bool IsCardTab { get; set; } 
    }

    public class DictionaryViewModelBase<TEntity> : ViewModelBase
    {
        public bool IsFilterDrawerOpened { get; set; } = true;
    }

    public class EmployeeDictionaryViewModel : DictionaryViewModelBase<Employee>
    {
        public PaginatorViewModel PaginatorViewModel { get; private set; }
        public EmployeeDictionaryViewModel(PaginatorViewModel paginatorViewModel)
        {
            PaginatorViewModel = paginatorViewModel;
        }
        public List<Employee> Entities { get; set; } = new List<Employee>(new []{new Employee{Name = "Василий"}});
    }

    public class CardDictionaryViewModel : DictionaryViewModelBase<Employee>
    {
        public List<Card> Entities { get; set; } = new List<Card>(new[] { new Card{WorkLoadTimeWednesday = 843, Employee = new Employee{Name = "Василий" } }});
    }
}