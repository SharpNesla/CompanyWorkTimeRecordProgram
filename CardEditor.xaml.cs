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

        public short WorkLoadTimeMonday
        {
            get { return Entity.WorkLoadTimeMonday; }
            set { Entity.WorkLoadTimeMonday = value; }
        }

        public short WorkLoadTimeTuesday
        {
            get { return Entity.WorkLoadTimeTuesday; }
            set { Entity.WorkLoadTimeTuesday = value; }
        }

        public short WorkLoadTimeWednesday
        {
            get { return Entity.WorkLoadTimeWednesday; }
            set { Entity.WorkLoadTimeWednesday = value; }
        }

        public short WorkLoadTimeThursday
        {
            get { return Entity.WorkLoadTimeThursday; }
            set { Entity.WorkLoadTimeThursday = value; }
        }

        public short WorkLoadTimeFriday
        {
            get { return Entity.WorkLoadTimeFriday; }
            set { Entity.WorkLoadTimeFriday = value; }
        }

        public decimal Payment
        {
            get { return Entity.Payment; }
            set { Entity.Payment = value; }
        }

        [DependsOn(nameof(WorkLoadTimeMonday),
            nameof(WorkLoadTimeTuesday),
            nameof(WorkLoadTimeWednesday),
            nameof(WorkLoadTimeThursday),
            nameof(WorkLoadTimeFriday))]
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
                var sumWorkTime = (short) (allHours * 100 + (allMinutes / 60) * 100 + allMinutes % 60);
                this.Entity.SumWorkLoadTime = sumWorkTime;
                return sumWorkTime;
            }
        }

        [DependsOn(nameof(Payment), nameof(SumWorkLoadTime))]
        public decimal PaymentFull
        {
            get
            {
                var hours = SumWorkLoadTime / 100;
                var minutes = SumWorkLoadTime % 100;

                if (hours >= 40)
                {
                    var overTimeHours = hours - 40;
                    var overTimedPayment =
                        40 * Payment + (overTimeHours * Payment + minutes * Payment / 60) * (decimal) 1.5;
                    this.Entity.PaymentFull = overTimedPayment;
                    return overTimedPayment;
                }

                var regularPayment = hours * Payment + minutes * Payment / 60;
                this.Entity.PaymentFull = regularPayment;
                return regularPayment;
            }
        }

        public bool IsNew { get; set; } = true;
        public EmployeeComboBoxViewModel EmployeeComboBoxViewModel { get; set; }

        public virtual string EditorTitle
        {
            get
            {
                return !IsNew
                    ? $"Редактирование информации о карточке загруженности №{Entity.Id}"
                    : $"Добавление информации о карточке загруженности";
            }
        }

        public Card Entity { get; set; }

        public CardEditorViewModel(IShell shell, CardService cards, EmployeeService employees)
        {
            this._shell = shell;
            this._cards = cards;

            this.EmployeeComboBoxViewModel = new EmployeeComboBoxViewModel(employees,
                x => Entity.Employee = x);

            if (shell.LastNavigatedParameter == null)
            {
                this.Entity = new Card {DatePass = DateTime.Today};
            }
            else
            {
                try
                {
                    this.Entity = cards.GetById((int) shell.LastNavigatedParameter);
                    this.EmployeeComboBoxViewModel.SelectedEntity = Entity.Employee;
                }
                catch (Exception e)
                {
                    _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                    return;
                }

                this.IsNew = false;
            }
        }

        public override void Apply()
        {
            if (this.Entity.Employee == null)
            {
                this._shell.MessageQueue.Enqueue("Поле работник не может быть незаполненным");
            }

            try
            {
                if (IsNew)
                {
                    _cards.Add(this.Entity);
                }
                else
                {
                    _cards.Update(this.Entity);
                }
            }
            catch (Exception e)
            {
                _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                return;
            }

            _shell.NavigateByUri(CompanyUris.Hub);
        }

        public override void OnIncorrectData()
        {
            this._shell.MessageQueue.Enqueue("Необходимые поля не заполнены или заполнены некорректно");
        }
    }
}