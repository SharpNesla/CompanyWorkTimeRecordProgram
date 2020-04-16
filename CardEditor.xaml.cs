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

namespace Employees
{
    /// <summary>
    /// Interaction logic for EmployeeCardEditor.xaml
    /// </summary>
   

    public class CardEditorViewModel
    {

        public bool IsNew { get; set; }
        public virtual string EditorTitle
        {
            get { return !IsNew ? $"Редактирование информации о карточке загруженности №{Entity.Id}" : $"Добавление новой карточки загруженности"; }
        }
        public Card Entity { get; set; } = new Card();
    }
}
