using MauiAppAgrar.ViewModels;

namespace MauiAppAgrar.Views;

public partial class TeruletekPage : ContentPage
{
    public TeruletekPage(TeruletekViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}