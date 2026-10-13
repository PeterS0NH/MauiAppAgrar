using MauiAppAgrar.ViewModels;
namespace MauiAppAgrar.Views;
public partial class ErogepekPage : ContentPage
{
    public ErogepekPage(ErogepekViewModel vm) { InitializeComponent(); BindingContext = vm; }
}