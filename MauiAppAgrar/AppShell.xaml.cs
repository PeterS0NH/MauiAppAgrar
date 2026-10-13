using MauiAppAgrar.Views;

namespace MauiAppAgrar;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(UjMunkaPage), typeof(UjMunkaPage));
        Routing.RegisterRoute(nameof(TeruletekPage), typeof(TeruletekPage));
        Routing.RegisterRoute(nameof(ErogepekPage), typeof(ErogepekPage));
        Routing.RegisterRoute(nameof(MunkaeszkozokPage), typeof(MunkaeszkozokPage));
    }
}