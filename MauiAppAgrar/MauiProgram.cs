using MauiAppAgrar.Services;
using MauiAppAgrar.ViewModels;
using MauiAppAgrar.Views;

namespace MauiAppAgrar;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<AdatService>();

        // ViewModelek
        builder.Services.AddTransient<TeruletekViewModel>();
        builder.Services.AddTransient<UjMunkaViewModel>();
        builder.Services.AddTransient<ErogepekViewModel>();
        builder.Services.AddTransient<MunkaeszkozokViewModel>();
        builder.Services.AddTransient<MainViewModel>();

        // Oldalak
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<TeruletekPage>();
        builder.Services.AddTransient<UjMunkaPage>();
        builder.Services.AddTransient<ErogepekPage>();
        builder.Services.AddTransient<MunkaeszkozokPage>();

        return builder.Build();
    }
}