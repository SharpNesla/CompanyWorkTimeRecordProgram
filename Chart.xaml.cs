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
using employees.Model;
using LiveCharts;
using LiveCharts.Wpf;

namespace employees
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
        }
    }

    public class ChartViewModel : ViewModelBase
    {
        public WorkLoadFilterDefinition FilterDefinition { get; set; } = new WorkLoadFilterDefinition();

        public WorkLoadData[] WorkLoadData = new WorkLoadData[]
        {
            new WorkLoadData {Min = 845, Average = 900, Max = 945},
            new WorkLoadData {Min = 845, Average = 900, Max = 945},
            new WorkLoadData {Min = 845, Average = 900, Max = 945},
            new WorkLoadData {Min = 845, Average = 900, Max = 945},
            new WorkLoadData {Min = 845, Average = 900, Max = 945},
        };

        public SeriesCollection Values =>
            new SeriesCollection
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

        public Func<double, string> Formatter { get; set; } = value => value.ToString("#0:00");
        public string[] Labels => new[] {"Понедельник", "Вторник", "Среда", "Четверг", "Пятница"};
    }
}