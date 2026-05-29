using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Audiora.Admin.Services;
using Audiora.Admin.Helpers;

namespace Audiora.Admin.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty] private string email        = "";
    [ObservableProperty] private string errorMessage = "";
    [ObservableProperty] private bool   isLoading    = false;

    public LoginViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    public async Task LoginAsync(string password)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
        {
            ErrorMessage = "Preencha todos os campos.";
            return;
        }

        IsLoading    = true;
        ErrorMessage = "";

        try
        {
            var result = await _apiService.LoginAsync(Email, password);

            if (result?.Success == true && result.Data != null)
            {
                SessionHelper.SaveToken(result.Data.Token);
                SessionHelper.SaveUser(result.Data.Name, result.Data.Email);

                // Abre dashboard e fecha login
                var dashboard = new Views.DashboardWindow();
                dashboard.Show();
                App.Current.Windows[0].Close();
            }
            else
            {
                ErrorMessage = result?.ErrorMessage ?? "Credenciais inválidas.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro de conexão: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}