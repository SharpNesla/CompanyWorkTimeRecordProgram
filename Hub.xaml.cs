using System;
using System.Collections.Generic;
using System.IO;
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
using employees.Elements;
using employees.Model;
using Employees.Model;
using Microsoft.Win32;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

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
        public string SearchString { get; set; }
        public PaginatorViewModel PaginatorViewModel { get; private set; }

        public DictionaryViewModelBase(PaginatorViewModel paginatorViewModel)
        {
            PaginatorViewModel = paginatorViewModel;
        }
    }

    public class EmployeeDictionaryViewModel : DictionaryViewModelBase<Employee, EmployeeFilterDefinition>, IPaginable
    {
        private readonly IShell _shell;
        private readonly EmployeeService _service;

        public List<Employee> Entities { get; set; }

        public ICommand AddEmployeeCommand =>
            new RelayCommand(() => _shell.NavigateByUri(CompanyUris.EmployeeEditor));

        public ICommand EditEmployeeCommand =>
            new RelayCommand<Employee>(x => _shell.NavigateByUri(CompanyUris.EmployeeEditor, x.Id));

        public ICommand ViewEmployeeInfoCommand =>
            new RelayCommand<Employee>(
                x => _shell.OpenDialogByUri(CompanyUris.EmployeeInfo, true, x.Id));

        public ICommand DeleteEmployeeCommand =>
            new RelayCommand<Employee>(x => _shell.OpenDialogByUri(
                CompanyUris.DeleteDialog, false,
                () => StateChanged?.Invoke(),
                new object[] { x.Id, _service }));

        public EmployeeDictionaryViewModel(IShell shell, EmployeeService service, PaginatorViewModel paginatorViewModel)
            : base(paginatorViewModel)
        {
            _shell = shell;
            _service = service;
            this.PaginatorViewModel.RegisterPaginable(this, true);
        }

        public long Count => this._service.GetCount(SearchString, FilterDefinition);

        public void Refresh(int page, int elements)
        {
            this.Entities = this._service.Get(SearchString, "", true, FilterDefinition,
                elements, page * elements);
        }

        public event Action StateChanged;
    }

    public class CardDictionaryViewModel : DictionaryViewModelBase<Card, CardFilterDefinition>, IPaginable
    {
        private readonly IShell _shell;
        private readonly CardService _service;

        public List<Card> Entities { get; set; }

        public ICommand AddCardCommand =>
            new RelayCommand(() => _shell.NavigateByUri(CompanyUris.CardEditor));

        public ICommand EditCardCommand =>
            new RelayCommand<Card>(x => _shell.NavigateByUri(CompanyUris.CardEditor, x.Id));

        public ICommand ViewCardInfoCommand =>
            new RelayCommand<Card>(
                x => _shell.OpenDialogByUri(CompanyUris.CardInfo, true, x.Id));

        public ICommand DeleteCardCommand =>
            new RelayCommand<Card>(x => _shell.OpenDialogByUri(
                CompanyUris.DeleteDialog, false,
                () => StateChanged?.Invoke(),
                new object[] {x.Id, _service}));

        public CardDictionaryViewModel(IShell shell, CardService service, PaginatorViewModel paginatorViewModel)
            : base(paginatorViewModel)
        {
            _shell = shell;
            _service = service;
            this.PaginatorViewModel.RegisterPaginable(this, true);
        }

        public long Count => this._service.GetCount(SearchString, FilterDefinition);

        public void Refresh(int page, int elements)
        {
            this.Entities = this._service.Get(SearchString, "", true, FilterDefinition,
                elements, page * elements);
        }

        public event Action StateChanged;

        public void ExportToExcel()
        {
            var workbook = new XSSFWorkbook();

            IWorkbook preparedWorkBook = null; //PrepareWorkBook(workbook);

            var dialog = new SaveFileDialog
            {
                InitialDirectory = @"~/Documents",
                Title = $"Путь к экспортируемой таблице карточек загруженности",
                AddExtension = true,
                Filter = "Файлы Excel 2007 (*.xlsx)|*.xlsx|Все остальные файлы (*.*)|*.*"
            };
            if (dialog.ShowDialog() == true)
            {
                if (!File.Exists(dialog.FileName))
                {
                    File.Delete(dialog.FileName);
                }

                //запись в файл
                using (var fs = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write))
                {
                    preparedWorkBook.Write(fs);
                }
            }
        }
    }
}