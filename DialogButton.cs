using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace employees
{
    class DialogButton : Button
    {
        public Uri DialogUri { get; set; }

        public DialogButton()
        {
            Click += NavigateButton_Click;
        }

        void NavigateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogHost.Show(Application.LoadComponent(DialogUri) as Page);
        }
    }
}
