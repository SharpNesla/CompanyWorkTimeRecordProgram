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
using MaterialDesignThemes.Wpf;

namespace Employees
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class Shell : System.Windows.Navigation.NavigationWindow
    {
        public Shell()
        {
            InitializeComponent();
        }
    }

    public static class CompanyUris
    {
        public static readonly Uri EmployeeEditor = new Uri("EmployeeEditor.xaml", UriKind.Relative);
        public static readonly Uri EmployeeInfo = new Uri("EmployeeInfo.xaml", UriKind.Relative);
        public static readonly Uri CardEditor = new Uri("CardEditor.xaml", UriKind.Relative);
        public static readonly Uri CardInfo = new Uri("CardInfo.xaml", UriKind.Relative);
        public static readonly Uri DeleteDialog = new Uri("DeleteDialog.xaml", UriKind.Relative);
        public static readonly Uri Hub = new Uri("Hub.xaml", UriKind.Relative);
    }

    public interface IShell
    {
        object LastNavigatedParameter { get; }

        object LastNavigatedDialogParameter { get; }
        void NavigateByUri(Uri uri, object parameter = null);
        void OpenDialogByUri(Uri uri, bool closeByClickAway = false, object parameter = null);
        void OpenDialogByUri(Uri uri, bool closeByClickAway = false, Action onDialogCloseCallback = null,
            object parameter = null);
        void TryCloseDialog();
        void CloseDialogImmediately();
    }

    public class ShellWindowViewModel : IShell
    {
        private Shell _windowDependencyObject;
        private DialogHost _host;

        public ICommand InitializeWindow =>
            new RelayCommand<Shell>(x => this._windowDependencyObject = x);

        public ICommand InitializeDialogHost =>
            new RelayCommand<DialogHost>(ExecuteAction);

        private void ExecuteAction(DialogHost x)
        {
            this._host = x;
        }

        public object LastNavigatedParameter { get; private set; }
        public object LastNavigatedDialogParameter { get; private set; }
        public Action OnDialogCloseCallback { get; set; }
        public async void NavigateByUri(Uri uri, object parameter = null)
        {
            await Task.Delay(220);
            this.LastNavigatedParameter = parameter;
            _windowDependencyObject.NavigationService.Navigate(uri);
        }

        public async void OpenDialogByUri(Uri uri, bool closeByClickAway = false, object parameter = null)
        {
            await Task.Delay(220);
            var dialogFrame = this._host.DialogContent as Frame;
            this.LastNavigatedDialogParameter = parameter;
            dialogFrame.NavigationService.Navigate(uri);
            this._host.IsOpen = true;
        }
      
        public async void OpenDialogByUri(Uri uri, bool closeByClickAway = false, Action onDialogCloseCallback = null,
            object parameter = null)
        {
            await Task.Delay(220);
            var dialogFrame = this._host.DialogContent as Frame;
            this.LastNavigatedDialogParameter = parameter;
            dialogFrame.NavigationService.Navigate(uri);
            this._host.IsOpen = true;
            this.OnDialogCloseCallback = onDialogCloseCallback;
        }


        public async void TryCloseDialog()
        {
            var dialogFrame = this._host.DialogContent as Frame;
            if (dialogFrame.CanGoBack)
            {
                dialogFrame.NavigationService.GoBack();
                dialogFrame.RemoveBackEntry();
            }
            else
            {
                this._host.IsOpen = false;
            }

            OnDialogCloseCallback?.Invoke();
            this.OnDialogCloseCallback = null;
        }

        public async void CloseDialogImmediately()
        {
            this.OnDialogCloseCallback = null;
        }
    }
}