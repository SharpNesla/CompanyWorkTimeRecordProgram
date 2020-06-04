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
        public static readonly Uri Auth = new Uri("Auth.xaml", UriKind.Relative);
        public static readonly Uri ConnectionLost = new Uri("ConnectionLostDialog.xaml", UriKind.Relative);
    }
    /// <summary>
    /// Интерфейс, через который происходит взаимодействие
    /// компонентов приложения и ViewModel-и главного окна 
    /// </summary>
    public interface IShell
    {
        SnackbarMessageQueue MessageQueue { get; }
        object LastNavigatedParameter { get; }
        object LastNavigatedDialogParameter { get; }
        /// <summary>
        /// Переход по адресу формы
        /// </summary>
        /// <param name="uri">URI-адрес формы</param>
        /// <param name="parameter">Объект-параметр</param>
        void NavigateByUri(Uri uri, object parameter = null);
        /// <summary>
        /// Открытие диалога по адресу формы
        /// </summary>
        /// <param name="uri">URI-адрес формы диалога</param>
        /// <param name="closeByClickAway">Флаг возможности выхода из диалога по клику вне области</param>
        /// <param name="parameter">Параметр</param>
        void OpenDialogByUri(Uri uri, bool closeByClickAway = false, object parameter = null);

        void OpenDialogByUri(Uri uri, bool closeByClickAway = false, Action onDialogCloseCallback = null,
            object parameter = null);
        /// <summary>
        /// Закрытие открытого диалога
        /// </summary>
        void CloseDialog();
    }
    /// <summary>
    /// ViewModel-прослойка, обеспечивающая логику
    /// работы окна приложения
    /// </summary>
    public class ShellWindowViewModel : ViewModelBase, IShell
    {
        private Shell _windowDependencyObject;
        private DialogHost _host;

        public SnackbarMessageQueue MessageQueue { get; set; }
            = new SnackbarMessageQueue(new TimeSpan(0, 0, 0, 1)) {IgnoreDuplicate = false};

        public ICommand InitializeWindow =>
            new RelayCommand<Shell>(x =>
            {
                if (x != null) this._windowDependencyObject = x;
                x.NavigationService.Navigate(CompanyUris.Auth);
            });

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
            this.LastNavigatedDialogParameter = parameter;
            _host.CloseOnClickAway = closeByClickAway;
            _host.CurrentSession?.Close();
            await Task.Delay(400);
            await _host.ShowDialog(Application.LoadComponent(uri) as Page);
        }

        public async void OpenDialogByUri(Uri uri, bool closeByClickAway = false, Action onDialogCloseCallback = null,
            object parameter = null)
        {
            await Task.Delay(220);
            this.LastNavigatedDialogParameter = parameter;
            _host.CloseOnClickAway = closeByClickAway;
            this.OnDialogCloseCallback = onDialogCloseCallback;
            await _host.ShowDialog(Application.LoadComponent(uri) as Page);
        }


        public void CloseDialog()
        {
            _host.CurrentSession.Close();
            OnDialogCloseCallback?.Invoke();
            this.OnDialogCloseCallback = null;
        }
        
        public ShellWindowViewModel()
        {
        }
    }
}