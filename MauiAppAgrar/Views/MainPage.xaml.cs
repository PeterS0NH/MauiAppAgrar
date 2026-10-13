using MauiAppAgrar.ViewModels;

namespace MauiAppAgrar.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.AdatokFrissitese();
    }

    private async void OnUjMunkaClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(UjMunkaPage));
    private async void OnErogepekClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(ErogepekPage));
    private async void OnMunkaeszkozokClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(MunkaeszkozokPage));
    private async void OnTeruletekClicked(object sender, EventArgs e) => await Shell.Current.GoToAsync(nameof(TeruletekPage));
}