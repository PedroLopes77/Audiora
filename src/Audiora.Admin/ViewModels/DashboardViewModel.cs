using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Audiora.Admin.Models;
using Audiora.Admin.Services;
using Audiora.Admin.Helpers;

namespace Audiora.Admin.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
    private readonly ApiService _apiService;

    [ObservableProperty] private ObservableCollection<MusicModel> musics      = new();
    [ObservableProperty] private bool                              isLoading   = false;
    [ObservableProperty] private string                            userName    = "";
    [ObservableProperty] private int                               totalMusics = 0;
    [ObservableProperty] private string                            statusMsg   = "";

    public DashboardViewModel(ApiService apiService)
    {
        _apiService = apiService;
        UserName    = SessionHelper.GetUserName();
    }

    [RelayCommand]
    public async Task LoadMusicsAsync()
    {
        IsLoading = true;
        StatusMsg = "";
        try
        {
            var result = await _apiService.GetMusicsAsync();
            if (result?.Success == true && result.Data != null)
            {
                Musics.Clear();
                foreach (var m in result.Data.Data)
                    Musics.Add(m);
                TotalMusics = result.Data.TotalCount;
                StatusMsg = $"{TotalMusics} músicas carregadas.";
            }
            else
            {
                StatusMsg = "Erro ao carregar músicas.";
            }
        }
        catch (Exception ex)
        {
            StatusMsg = $"Erro: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task DeleteMusicAsync(MusicModel music)
    {
        var confirm = MessageBox.Show(
            $"Deseja deletar '{music.Title}'?",
            "Confirmar exclusão",
            MessageBoxButton.YesNo,
            MessageBoxImage.Warning);

        if (confirm != MessageBoxResult.Yes) return;

        bool success = await _apiService.DeleteMusicAsync(music.Id);
        if (success)
        {
            Musics.Remove(music);
            TotalMusics--;
            StatusMsg = $"'{music.Title}' removida com sucesso.";
        }
        else
        {
            StatusMsg = "Erro ao deletar música.";
        }
    }

    [RelayCommand]
    public void Logout()
    {
        SessionHelper.Logout();
        var login = new Views.LoginWindow();
        login.Show();
        App.Current.Windows[0].Close();
    }
}