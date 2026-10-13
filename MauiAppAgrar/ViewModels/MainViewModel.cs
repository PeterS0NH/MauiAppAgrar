using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppAgrar.Models;
using MauiAppAgrar.Services;
using MauiAppAgrar.Views;
using System.Collections.ObjectModel;

namespace MauiAppAgrar.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly AdatService _adatService;

    [ObservableProperty]
    ObservableCollection<Munka> elvegzettMunkak;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(TorlesCommand))]
    [NotifyCanExecuteChangedFor(nameof(ModositasCommand))]
    Munka kijeloltMunka;

    public bool VanKijeloles => KijeloltMunka != null;

    public MainViewModel(AdatService adatService)
    {
        _adatService = adatService;
        ElvegzettMunkak = new ObservableCollection<Munka>(_adatService.Munkak);
    }

    public void AdatokFrissitese()
    {
        ElvegzettMunkak = new ObservableCollection<Munka>(_adatService.Munkak);

        KijeloltMunka = null;
    }

    [RelayCommand(CanExecute = nameof(VanKijeloles))]
    async Task Torles()
    {
        if (KijeloltMunka == null) return;

        bool valasz = await Shell.Current.DisplayAlert("Törlés", "Biztosan törölni szeretnéd ezt a munkát?", "Igen", "Nem");
        if (!valasz) return;

        _adatService.Munkak.Remove(KijeloltMunka);

        AdatokFrissitese();

        _adatService.Mentes();
    }

    [RelayCommand(CanExecute = nameof(VanKijeloles))]
    async Task Modositas()
    {
        if (KijeloltMunka == null) return;

        var navigaciosAdatok = new Dictionary<string, object>
        {
            { "Szerkesztes", KijeloltMunka }
        };

        await Shell.Current.GoToAsync(nameof(UjMunkaPage), navigaciosAdatok);
    }
}