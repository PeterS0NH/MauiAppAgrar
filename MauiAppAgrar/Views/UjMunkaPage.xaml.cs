using MauiAppAgrar.ViewModels;

namespace MauiAppAgrar.Views;

public partial class UjMunkaPage : ContentPage
{
    public UjMunkaPage(UjMunkaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}