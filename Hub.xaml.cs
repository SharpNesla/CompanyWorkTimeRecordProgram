﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
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
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику
    /// работы главной формы
    /// </summary>
    public class HubViewModel : ViewModelBase
    {
        public bool IsEmployeeTab { get; set; } = true;
        public bool IsCardTab { get; set; }

        public ICommand ExportEmployeesToExcel { get; }
        public ICommand ExportCardsToExcel { get; }

        public EmployeeDictionaryViewModel EmployeeDictionaryViewModel { get; }
        public CardDictionaryViewModel CardDictionaryViewModel { get; }

        public HubViewModel(EmployeeDictionaryViewModel employeeDictionaryViewModel,
            CardDictionaryViewModel cardDictionaryViewModel)
        {
            EmployeeDictionaryViewModel = employeeDictionaryViewModel;
            ExportCardsToExcel =
                new RelayCommand(() => this.CardDictionaryViewModel.ExportToExcel());
            CardDictionaryViewModel = cardDictionaryViewModel;
            ExportEmployeesToExcel =
                new RelayCommand(() => this.EmployeeDictionaryViewModel.ExportToExcel());
        }
    }

    public class DictionaryViewModelBase<TEntity, TFilter> : ViewModelBase where TFilter : new()
    {
        protected bool SortDirection = true;
        protected string SortingColumn;
        protected bool IsDisconnected;

        public bool IsFilterDrawerOpened { get; set; }
        public TFilter FilterDefinition { get; set; } = new TFilter();
        public virtual ICommand EraseFilters => new RelayCommand(() => this.FilterDefinition = new TFilter());
        public string SearchString { get; set; }
        public PaginatorViewModel PaginatorViewModel { get; }

        public DictionaryViewModelBase(PaginatorViewModel paginatorViewModel)
        {
            PaginatorViewModel = paginatorViewModel;
        }
    }
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику
    /// работы вкладки списка работников.
    /// </summary>
    public class EmployeeDictionaryViewModel : DictionaryViewModelBase<Employee, EmployeeFilterDefinition>, IPaginable
    {
        private readonly IShell _shell;
        private readonly EmployeeService _service;
        public bool IsWriteRights => _service.CurrentUser.Role == Role.Manager;
        public List<Employee> Entities { get; set; }

        public ICommand AddEmployeeCommand =>
            new RelayCommand(() => _shell.NavigateByUri(CompanyUris.EmployeeEditor));

        public ICommand EditEmployeeCommand =>
            new RelayCommand<Employee>(x => _shell.NavigateByUri(CompanyUris.EmployeeEditor, x.Id));

        public ICommand ViewEmployeeInfoCommand =>
            new RelayCommand<Employee>(
                x => _shell.OpenDialogByUri(CompanyUris.EmployeeInfo, true, x.Id));

        public ICommand SortEmployeeCommand => new RelayCommand<DataGridSortingEventArgs>(
            eventArgs =>
            {
                SortDirection = eventArgs.Column.SortDirection == ListSortDirection.Ascending;
                SortingColumn = eventArgs.Column.SortMemberPath.Substring(1);
                StateChanged?.Invoke();
                eventArgs.Column.SortDirection =
                    SortDirection ? ListSortDirection.Ascending : ListSortDirection.Descending;
            });

        public ICommand RefreshCommand => new RelayCommand(() => this.StateChanged?.Invoke());

        public ICommand DeleteEmployeeCommand =>
            new RelayCommand<Employee>(x => _shell.OpenDialogByUri(
                CompanyUris.DeleteDialog, false,
                () => StateChanged?.Invoke(),
                new object[] {x.Id, _service}));

        public EmployeeDictionaryViewModel(IShell shell, EmployeeService service, PaginatorViewModel paginatorViewModel)
            : base(paginatorViewModel)
        {
            _shell = shell;
            _service = service;
            this.PaginatorViewModel.RegisterPaginable(this, true);
        }

        public long Count
        {
            get
            {
                try
                {
                    return this._service.GetCount(SearchString, FilterDefinition);
                }
                catch (Exception)
                {
                    IsDisconnected = true;
                    _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                    return 0;
                }
            }
        }

        public void Refresh(int page, int elements)
        {
            if (!IsDisconnected)
            {

                this.Entities = this._service.Get(SearchString, SortingColumn,
                    SortDirection, FilterDefinition,
                    elements, page * elements);
            }
        }

        public event Action StateChanged;

        public void ExportToExcel()
        {
            try
            {
                this._service.SaveExcelDocument(SearchString, SortingColumn, SortDirection, FilterDefinition);
            }
            catch (Exception)
            {
                IsDisconnected = true;
                _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
            }
        }
    }
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику
    /// работы вкладки списка карточек
    /// </summary>
    public class CardDictionaryViewModel : DictionaryViewModelBase<Card, CardFilterDefinition>, IPaginable
    {
        private readonly IShell _shell;
        private readonly EmployeeService _employeeService;
        private readonly CardService _service;

        public bool IsWriteRights => _employeeService.CurrentUser.Role == Role.Manager;
        public List<Card> Entities { get; set; }
        public EmployeeComboBoxViewModel EmployeeComboBoxViewModel { get; set; }
        [DependsOn(nameof(EraseFilters))]
        public bool IsByEmployee
        {
            get { return this.FilterDefinition.IsByEmployee; }

            set
            {
                this.EmployeeComboBoxViewModel.IsEnabled = value;
                this.FilterDefinition.IsByEmployee = value;
            }
        }

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

        public ICommand SortEmployeeCommand => new RelayCommand<DataGridSortingEventArgs>(
            eventArgs =>
            {
                SortDirection = eventArgs.Column.SortDirection == ListSortDirection.Ascending;
                SortingColumn = eventArgs.Column.SortMemberPath.Substring(1);
                StateChanged?.Invoke();
                eventArgs.Column.SortDirection =
                    SortDirection ? ListSortDirection.Ascending : ListSortDirection.Descending;
            });
        
        public override ICommand EraseFilters => new RelayCommand(EraseFiltersAction);

        private void EraseFiltersAction()
        {
            this.FilterDefinition = new CardFilterDefinition();
            this.IsByEmployee = false;
            OnPropertyChanged(nameof(IsByEmployee));
            this.EmployeeComboBoxViewModel = new EmployeeComboBoxViewModel(_employeeService,
                    x => this.FilterDefinition.EmployeeId = x?.Id, false)
                { IsEnabled = false };
        }

        public ICommand RefreshCommand => new RelayCommand(() => this.StateChanged?.Invoke());

        public CardDictionaryViewModel(IShell shell, EmployeeService employeeService,
            CardService cardService, PaginatorViewModel paginatorViewModel)
            : base(paginatorViewModel)
        {
            _shell = shell;
            _employeeService = employeeService;
            _service = cardService;
            this.EmployeeComboBoxViewModel =
                new EmployeeComboBoxViewModel(employeeService,
                    x => this.FilterDefinition.EmployeeId = x?.Id, false)
                    {IsEnabled = false};
            this.PaginatorViewModel.RegisterPaginable(this, true);
        }

        public long Count
        {
            get
            {
                try
                {
                    return this._service.GetCount(SearchString, FilterDefinition);
                }
                catch (Exception)
                {
                    IsDisconnected = true;
                    _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
                    return 0;
                }
            }
        }

        public void Refresh(int page, int elements)
        {
            if (!IsDisconnected)
            {
                this.Entities = this._service.Get(SearchString, SortingColumn, SortDirection, FilterDefinition,
                    elements, page * elements);
            }
        }

        public event Action StateChanged;

        public void ExportToExcel()
        {
            try
            {
                this._service.SaveExcelDocument(SearchString, "", true, FilterDefinition);
            }
            catch (Exception)
            {
                IsDisconnected = true;
                _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
            }
        }
    }
}