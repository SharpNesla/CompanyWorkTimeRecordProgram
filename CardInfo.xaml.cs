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
using employees.Model;
using Employees.Model;
using employees.ViewModelBases;

namespace Employees
{

    public class CardInfoViewModel : ViewModelBase
    {
        public Card Entity { get; set; }
        public List<Card> CardArray => new List<Card>(new[] {this.Entity});

        public CardInfoViewModel(IShell shell, CardService service)
        {
            this.Entity = service.GetById((int) shell.LastNavigatedDialogParameter);
        }
    }
}