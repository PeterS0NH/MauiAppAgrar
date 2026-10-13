using MauiAppAgrar.ViewModels;
namespace MauiAppAgrar.Views;
public partial class MunkaeszkozokPage : ContentPage
{
    public MunkaeszkozokPage(MunkaeszkozokViewModel vm) { InitializeComponent(); BindingContext = vm; }
}