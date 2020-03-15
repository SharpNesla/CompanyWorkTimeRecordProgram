using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Employees{
    public class NavigateButton : Button
    {
        public Uri NavigateUri { get; set; }

        public NavigateButton()
        {
            Click += NavigateButton_Click;
        }

        void NavigateButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var navigationService = NavigationService.GetNavigationService(this);
            if (navigationService != null)
                navigationService.Navigate(NavigateUri);
        }
    }
}
