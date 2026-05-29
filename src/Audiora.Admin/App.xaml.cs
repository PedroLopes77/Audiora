using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Audiora.Admin.Services;
using Audiora.Admin.ViewModels;

namespace Audiora.Admin;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var collection = new ServiceCollection();

        // Services
        collection.AddSingleton<ApiService>();

        // ViewModels
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<DashboardViewModel>();

        Services = collection.BuildServiceProvider();
    }
}