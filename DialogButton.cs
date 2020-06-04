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

        async void NavigateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //var window = new Window();
            //window.WindowStyle = WindowStyle.ToolWindow;
            //window.SizeToContent = SizeToContent.WidthAndHeight;
            //window.Content = Application.LoadComponent(DialogUri) as Page;
            //window.ShowDialog();
            await DialogHost.Show(Application.LoadComponent(DialogUri) as Page);
        }
    }
}
