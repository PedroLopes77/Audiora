using System.Windows;
using Audiora.Admin.ViewModels;

namespace Audiora.Admin.Views;

public partial class LoginWindow : Window
{
    private readonly LoginViewModel _vm;

    public LoginWindow()
    {
        InitializeComponent();
        _vm = App.Services.GetService(typeof(LoginViewModel)) as LoginViewModel
              ?? new LoginViewModel(App.Services.GetService(typeof(Services.ApiService)) as Services.ApiService
                 ?? new Services.ApiService());
        DataContext = _vm;

        _vm.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(_vm.ErrorMessage))
            {
                TxtError.Text       = _vm.ErrorMessage;
                TxtError.Visibility = string.IsNullOrEmpty(_vm.ErrorMessage)
                    ? Visibility.Collapsed : Visibility.Visible;
            }
            if (e.PropertyName == nameof(_vm.IsLoading))
            {
                TxtLoading.Visibility = _vm.IsLoading ? Visibility.Visible : Visibility.Collapsed;
                BtnLogin.IsEnabled    = !_vm.IsLoading;
            }
        };
    }

    private async void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        _vm.Email = TxtEmail.Text;
        await _vm.LoginAsync(TxtPassword.Password);
    }
}