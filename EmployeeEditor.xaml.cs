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
using Employees.Model;

namespace employees
{
    /// <summary>
    /// Interaction logic for EmployeeEditor.xaml
    /// </summary>
    public partial class EmployeeEditor
    {
        public EmployeeEditor()
        {
            InitializeComponent();
            
        }
    }

    public class EmployeeEditorViewModel
    {
        public bool IsNew { get; set; } = false;
        public virtual string EditorTitle
        {
            get { return !IsNew ? $"Редактирование информации о работнике №{Entity.Id}" : $"Добавление информации о работнике"; }
        }

        public bool IsPasswordChanging { get; set; }
        public Employee Entity { get; set; } = new Employee(){DateBirth = DateTime.Today};
    }
}
