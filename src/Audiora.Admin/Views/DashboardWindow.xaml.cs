using System.Windows;
using Audiora.Admin.ViewModels;

namespace Audiora.Admin.Views;

public partial class DashboardWindow : Window
{
    private readonly DashboardViewModel _vm;

    public DashboardWindow()
    {
        InitializeComponent();
        _vm = App.Services.GetService(typeof(DashboardViewModel)) as DashboardViewModel
              ?? new DashboardViewModel(App.Services.GetService(typeof(Services.ApiService)) as Services.ApiService
                 ?? new Services.ApiService());
        DataContext = _vm;

        Loaded += async (s, e) => await _vm.LoadMusicsAsync();
    }
}