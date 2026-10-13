using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using MauiAppAgrar.Models;
using MauiAppAgrar.Services;

namespace MauiAppAgrar.ViewModels;

public partial class TeruletekViewModel : ObservableObject
{
    private readonly AdatService _adatService;

    [ObservableProperty]
    ObservableCollection<Terulet> teruletek;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(MegosztasCommand))]
    [NotifyCanExecuteChangedFor(nameof(TorlesCommand))]
    Terulet kijeloltTerulet;

    [ObservableProperty] string ujNev;
    [ObservableProperty] double ujMeret;
    [ObservableProperty] string ujTulaj;

    public bool VanKijeloles => KijeloltTerulet != null;

    public TeruletekViewModel(AdatService adatService)
    {
        _adatService = adatService;

        if (_adatService.Teruletek.Count == 0)
        {
            _adatService.Teruletek.Add(new Terulet { Nev = "Minta Terület", NagysagHa = 10, Tulajdonos = "Rendszer" });
        }
        Teruletek = new ObservableCollection<Terulet>(_adatService.Teruletek);
    }

    [RelayCommand]
    async Task Hozzaad()
    {
        if (string.IsNullOrWhiteSpace(UjNev)) return;

        var uj = new Terulet { Nev = UjNev, NagysagHa = UjMeret, Tulajdonos = UjTulaj };
        _adatService.Teruletek.Add(uj);
        Teruletek.Add(uj);

        _adatService.Mentes();

        UjNev = string.Empty;
        UjMeret = 0;
        UjTulaj = string.Empty;
    }

    [RelayCommand(CanExecute = nameof(VanKijeloles))]
    void Torles()
    {
        if (KijeloltTerulet == null) return;

        _adatService.Teruletek.Remove(KijeloltTerulet);
        Teruletek.Remove(KijeloltTerulet);

        _adatService.Mentes();

        KijeloltTerulet = null;
    }

    [RelayCommand(CanExecute = nameof(VanKijeloles))]
    async Task Megosztas()
    {
        if (KijeloltTerulet == null) return;

        await Share.Default.RequestAsync(new ShareTextRequest
        {
            Text = $"Terület: {KijeloltTerulet.Nev}\nMéret: {KijeloltTerulet.NagysagHa} ha\nTulajdonos: {KijeloltTerulet.Tulajdonos}",
            Title = "Terület megosztása"
        });
    }
}