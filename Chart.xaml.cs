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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Employees;
using employees.Elements;
using employees.Model;
using LiveCharts;
using LiveCharts.Wpf;

namespace employees
{
    public class ChartViewModel : ViewModelBase
    {
        private readonly IShell _shell;
        public EmployeeComboBoxViewModel EmployeeComboBoxViewModel { get; set; }
        public WorkLoadFilterDefinition FilterDefinition { get; set; } = new WorkLoadFilterDefinition();

        public List<WorkLoadData> WorkLoadData;

        [DependsOn(nameof(WorkLoadData))]
        public SeriesCollection Values { get; private set; }
        public bool IsByEmployee
        {
            get => this.FilterDefinition.IsByEmployee;

            set
            {
                this.EmployeeComboBoxViewModel.IsEnabled = value;
                this.FilterDefinition.IsByEmployee = value;
            }
        }

        public ICommand EraseFilters =>
            new RelayCommand(() => this.FilterDefinition = new WorkLoadFilterDefinition());

        public ICommand RefreshCommand { get; }
        public Func<double, string> Formatter { get; set; } = value => value.ToString("#0:00");
        public string[] Labels => new[] {"Понедельник", "Вторник", "Среда", "Четверг", "Пятница"};
        void Refresh(CardService service)
        {
            try
            {
                WorkLoadData = service.GetWorkLoadData(FilterDefinition);
                Values = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        FontFamily =
                            new FontFamily(
                                "pack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto"),
                        FontWeight = FontWeights.Normal,
                        FontSize = 14,
                        DataLabels = true,
                        Title = "Минимальное",
                        Values = new ChartValues<int>(WorkLoadData.Select(x => x.Min))
                    },
                    new ColumnSeries
                    {
                        FontFamily =
                            new FontFamily(
                                "pack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto"),
                        FontWeight = FontWeights.Normal,
                        FontSize = 14,
                        DataLabels = true,
                        Title = "Среднее",
                        Values = new ChartValues<int>(WorkLoadData.Select(x => x.Average))
                    },
                    new ColumnSeries
                    {
                        FontFamily =
                            new FontFamily(
                                "pack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto"),
                        FontWeight = FontWeights.Normal,
                        FontSize = 14,
                        DataLabels = true,
                        Title = "Максимальное",
                        Values = new ChartValues<int>(WorkLoadData.Select(x => x.Max))
                    }
                };
            }
            catch (Exception e)
            {
                _shell.OpenDialogByUri(CompanyUris.ConnectionLost, false, null);
            }

            
        }
        public ChartViewModel(IShell shell, CardService service, EmployeeService employeeService)
        {
            _shell = shell;
            this.EmployeeComboBoxViewModel =
                new EmployeeComboBoxViewModel(employeeService,
                        x => this.FilterDefinition.EmployeeId = x?.Id, false)
                    { IsEnabled = false };
            RefreshCommand = new RelayCommand(
                () => Refresh(service));
            Refresh(service);
        }

    }
}