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
using Employees;
using employees.Model;
using Employees.Model;

namespace employees
{
    public abstract class EditorBase : ViewModelBase
    {
        public ICommand ApplyCommand => new RelayCommand<DependencyObject>(ApplyChanges);

        /// <summary>
        /// Производит UpdateSource всех textbox и combobox
        /// в передаваемой view для обновления валидаций на полях
        /// и проверяет их на валидность
        /// </summary>
        /// <param name="view"></param>
        public virtual void ApplyChanges(DependencyObject view)
        {
            var tree = FindVisualChildren<TextBox>(view);
            var comboboxes = FindVisualChildren<ComboBox>(view).Where(x => x.IsEnabled);
            foreach (TextBox tb in tree.Where(x=>x.Visibility == Visibility.Visible && x.IsEnabled))
            {
                tb.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            }

            foreach (ComboBox tb in comboboxes)
            {
                tb.GetBindingExpression(ComboBox.SelectedItemProperty)?.UpdateSource();
            }


            foreach (TextBox tb in tree)
            {
                if (Validation.GetHasError(tb))
                {
                    OnIncorrectData();
                    return;
                }
            }

            foreach (var cb in comboboxes)
            {
                if (Validation.GetHasError(cb))
                {
                    OnIncorrectData();
                    return;
                }
            }

            this.Apply();
        }

        /// <summary>
        /// Применить изменения или добавить сущность
        /// в базу данных
        /// </summary>
        public abstract void Apply();

        public abstract void OnIncorrectData();
        /// <summary>
        /// Метод, позволяющий получить всех детей данного
        /// элемента в визуальном дереве элементов View
        /// </summary>
        /// <typeparam name="T">Тип отбираемого контрола</typeparam>
        /// <param name="depObj">View или её элемент</param>
        /// <returns>Перечислимое всех детей данного элемент</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T) child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }

    public class EmployeeEditorViewModel : EditorBase
    {
        private readonly IShell _shell;
        private readonly EmployeeService _employees;

        public Role Role
        {
            get { return Entity.Role; }
            set { Entity.Role = value; }
        }

        public bool HasRights => Role != Role.Undefined;

        public bool IsNew { get; set; } = true;

        public virtual string EditorTitle
        {
            get
            {
                return !IsNew
                    ? $"Редактирование информации о работнике №{Entity.Id}"
                    : $"Добавление информации о работнике";
            }
        }

        public bool IsPasswordChanging { get; set; }
        public Employee Entity { get; set; }

        public EmployeeEditorViewModel(IShell shell, EmployeeService employees)
        {
            _shell = shell;
            _employees = employees;
            if (shell.LastNavigatedParameter == null)
            {
                this.Entity = new Employee{DateBirth = DateTime.Now.AddYears(-16)};
            }
            else
            {
                this.Entity = employees.GetById((int) shell.LastNavigatedParameter);
                IsNew = false;
            }
        }

        public override void Apply()
        {
            if (IsNew)
            {
                _employees.Add(this.Entity);
            }
            else
            {
                _employees.Update(this.Entity);
            }

            _shell.NavigateByUri(CompanyUris.Hub);
        }

        public override void OnIncorrectData()
        {
            this._shell.MessageQueue.Enqueue("Не заполнены необходимые поля");
        }
    }
}