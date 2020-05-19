using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using employees.Model;
using Employees.Model;

namespace employees.Elements
{
    /// <summary>
    /// Interaction logic for EmployeeComboBox.xaml
    /// </summary>
    public partial class EmployeeComboBox : UserControl
    {
        public EmployeeComboBox()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// Базовый класс для динамических 
    /// полей со списком поисковых строк
    /// </summary>
    /// <typeparam name="TEntity">Сущность, по которой ведётся поиск</typeparam>
    /// <typeparam name="TRepository">Репозиторий сущности</typeparam>
    public class EmployeeComboBoxViewModel : ViewModelBase, IDataErrorInfo
    {
        protected readonly EmployeeService _service;

        private Employee _selectedEntity;
        private readonly bool _isRequired;
        private string _entityText;
        public bool IsDropdownOpen { get; set; }
        public ICommand OnInputChangedCommand => new RelayCommand(OnInputChanged);
        public List<Employee> Entities { get; set; }

        /// <summary>
        /// Строка поиска
        /// в геттере проверка на null,
        /// в сеттере при изменении строки триггерится поиск
        /// </summary>
        public string EntityText
        {
            get
            {
                if (SelectedEntity != null)
                {
                    _entityText = this.SelectedEntity.Signature;
                    return this.SelectedEntity.Signature;
                }
                else
                {
                    return _entityText;
                }
            }
            set
            {
                this.Entities =
                    _service.Get(value, "", true, new EmployeeFilterDefinition(), 10, 0);
            }
        }

        /// <summary>
        /// Выбранная сущность, при смене триггерит событие EntityChanged
        /// </summary>
        public Employee SelectedEntity
        {
            get { return _selectedEntity; }
            set
            {
                _selectedEntity = value;
                EntityChanged?.Invoke(value);
            }
        }

        public Action<Employee> EntityChanged;

        public EmployeeComboBoxViewModel(EmployeeService service, Action<Employee> OnEntityChangedCallback)
        {
            this._service = service;
            this.Entities = _service.Get("", "", true, new EmployeeFilterDefinition(), 10, 0);
            this.EntityChanged = OnEntityChangedCallback;
        }

        public void OnInputChanged()
        {
            IsDropdownOpen = true;
        }

        #region Реализация IDataErrorInfo для валидации

        public string this[string columnName]
        {
            get
            {
                var errorString = "";
                if (columnName == nameof(this.SelectedEntity) && _isRequired)
                {
                    if (this.SelectedEntity == null)
                    {
                        errorString = "Поле \"Работник\" не может быть пустым";
                    }
                }

                return errorString;
            }
        }

        public string Error { get; }

        #endregion
    }
}