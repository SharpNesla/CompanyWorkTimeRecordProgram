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
using employees.Elements;
using employees.Model;
using Employees.Model;
using PropertyChanged;

namespace Employees
{
    /// <summary>
    /// Interaction logic for EmployeeCardEditor.xaml
    /// </summary>


    public class CardEditorViewModel : EditorBase
    {
        private readonly IShell _shell;
        private readonly CardService _cards;
        
        public int SumWorkLoadTime
        {
            get
            {
                var allMinutes = this.Entity.WorkLoadTimeMonday % 100 +
                                 this.Entity.WorkLoadTimeTuesday % 100 +
                                 this.Entity.WorkLoadTimeWednesday % 100 +
                                 this.Entity.WorkLoadTimeThursday % 100 +
                                 this.Entity.WorkLoadTimeFriday % 100;

                var allHours = this.Entity.WorkLoadTimeMonday / 100 +
                               this.Entity.WorkLoadTimeTuesday / 100 +
                               this.Entity.WorkLoadTimeWednesday / 100 +
                               this.Entity.WorkLoadTimeThursday / 100 +
                               this.Entity.WorkLoadTimeFriday / 100;
                return allHours * 100 + (allMinutes / 60) * 100 + allMinutes % 60;
            }
        }
        
        public bool IsNew { get; set; } = true;
        public EmployeeComboBoxViewModel EmployeeComboBoxViewModel { get; set; }
        public virtual string EditorTitle
        {
            get { return !IsNew ? $"Редактирование информации о карточке загруженности №{Entity.Id}" : $"Добавление информации о карточке загруженности"; }
        }
        public Card Entity { get; set; }

        public CardEditorViewModel(IShell shell, CardService cards, EmployeeService employees)
        {
            this._shell = shell;
            this._cards = cards;

            this.EmployeeComboBoxViewModel = new EmployeeComboBoxViewModel(employees,
                x=>Entity.EmployeeId = x.Id);

            if (shell.LastNavigatedParameter == null)
            {
                this.Entity = new Card();
            }
            else
            {
                this.Entity = cards.GetById((int)shell.LastNavigatedParameter);
                this.EmployeeComboBoxViewModel.SelectedEntity = Entity.Employee;
                this.IsNew = false;
            }
        }

        public override void Apply()
        {
            if (IsNew)
            {
                _cards.Add(this.Entity);
            }
            else
            {
                _cards.Update(this.Entity);
            }
            _shell.NavigateByUri(CompanyUris.Hub);
        }

        public override void OnIncorrectData()
        {
        }
    }
}
