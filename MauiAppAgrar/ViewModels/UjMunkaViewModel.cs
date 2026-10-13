using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppAgrar.Models;
using MauiAppAgrar.Services;
using System.Collections.ObjectModel;

namespace MauiAppAgrar.ViewModels;

public partial class UjMunkaViewModel : ObservableObject, IQueryAttributable
{
    private readonly AdatService _adatService;

    public ObservableCollection<Erogep> Erogepek => new(_adatService.Erogepek);
    public ObservableCollection<Munkaeszkoz> Eszkozok => new(_adatService.Eszkozok);
    public ObservableCollection<Terulet> Teruletek => new(_adatService.Teruletek);

    private Munka _szerkesztendoMunka;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsStep1), nameof(IsStep2), nameof(IsStep3), nameof(IsStep4), nameof(IsStep5))]
    [NotifyPropertyChangedFor(nameof(VanVissza), nameof(VanTovabb), nameof(EzAzUtolsoOldal))]
    int jelenlegiPozicio = 0;

    public bool IsStep1 => JelenlegiPozicio == 0;
    public bool IsStep2 => JelenlegiPozicio == 1;
    public bool IsStep3 => JelenlegiPozicio == 2;
    public bool IsStep4 => JelenlegiPozicio == 3;
    public bool IsStep5 => JelenlegiPozicio == 4;
    public bool VanVissza => JelenlegiPozicio > 0;
    public bool VanTovabb => JelenlegiPozicio < 4;
    public bool EzAzUtolsoOldal => JelenlegiPozicio == 4;

    [ObservableProperty] string munkaTipus;
    [ObservableProperty] Erogep valasztottGep;
    [ObservableProperty] Munkaeszkoz valasztottEszkoz;
    [ObservableProperty] Terulet valasztottTerulet;
    [ObservableProperty] double kezdoUzemora;
    [ObservableProperty] double zaroUzemora;

    [ObservableProperty] DateTime kezdoDatum = DateTime.Today;
    [ObservableProperty] TimeSpan kezdoIdo = new TimeSpan(8, 0, 0);
    [ObservableProperty] TimeSpan zaroIdo = new TimeSpan(16, 0, 0);

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ZaroDatumLathato))]
    bool isTobbNapos = false;

    [ObservableProperty] DateTime zaroDatum = DateTime.Today;

    public bool ZaroDatumLathato => IsTobbNapos;

    public UjMunkaViewModel(AdatService adatService)
    {
        _adatService = adatService;
    }

    [RelayCommand]
    void ToggleTobbNapos() => IsTobbNapos = !IsTobbNapos;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("Szerkesztes"))
        {
            var munka = query["Szerkesztes"] as Munka;
            if (munka != null)
            {
                _szerkesztendoMunka = munka;
                MunkaTipus = munka.Tipus;
                KezdoUzemora = munka.KezdoUzemora;
                ZaroUzemora = munka.ZaroUzemora;
                KezdoDatum = munka.Kezdet.Date;
                KezdoIdo = munka.Kezdet.TimeOfDay;
                ZaroDatum = munka.Veg.Date;
                ZaroIdo = munka.Veg.TimeOfDay;
                IsTobbNapos = munka.Veg.Date > munka.Kezdet.Date;
                ValasztottGep = Erogepek.FirstOrDefault(g => g.Nev == munka.Erogep?.Nev);
                ValasztottEszkoz = Eszkozok.FirstOrDefault(e => e.Nev == munka.Munkaeszkoz?.Nev);
                ValasztottTerulet = Teruletek.FirstOrDefault(t => t.Nev == munka.Terulet?.Nev);
            }
        }
    }

    [RelayCommand] void LepesElore() { if (JelenlegiPozicio < 4) JelenlegiPozicio++; }
    [RelayCommand] void LepesVissza() { if (JelenlegiPozicio > 0) JelenlegiPozicio--; }

    [RelayCommand]
    async Task Mentés()
    {
        if (string.IsNullOrWhiteSpace(MunkaTipus))
        {
            await Shell.Current.DisplayAlert("Hiányzó adat", "Kérlek válassz munka típust (1. lépés)!", "OK");
            return;
        }
        if (ValasztottGep == null)
        {
            await Shell.Current.DisplayAlert("Hiányzó adat", "Kérlek válassz erőgépet (2. lépés)!", "OK");
            return;
        }
        if (ValasztottEszkoz == null)
        {
            await Shell.Current.DisplayAlert("Hiányzó adat", "Kérlek válassz munkaeszközt (3. lépés)!", "OK");
            return;
        }
        if (ValasztottTerulet == null)
        {
            await Shell.Current.DisplayAlert("Hiányzó adat", "Kérlek válassz területet (4. lépés)!", "OK");
            return;
        }

        DateTime teljesKezdet = KezdoDatum.Date + KezdoIdo;
        DateTime teljesVeg = (IsTobbNapos ? ZaroDatum.Date : KezdoDatum.Date) + ZaroIdo;

        if (teljesVeg <= teljesKezdet)
        {
            await Shell.Current.DisplayAlert("Hibás időpont", "A befejezés ideje nem lehet korábban (vagy ugyanakkor), mint a kezdés!", "OK");
            return;
        }

        if (ZaroUzemora < KezdoUzemora)
        {
            await Shell.Current.DisplayAlert("Hibás üzemóra", "A záró üzemóra nem lehet kisebb, mint a kezdő!", "OK");
            return;
        }

        if (_szerkesztendoMunka != null)
        {
            _szerkesztendoMunka.Tipus = MunkaTipus;
            _szerkesztendoMunka.Erogep = ValasztottGep;
            _szerkesztendoMunka.Munkaeszkoz = ValasztottEszkoz;
            _szerkesztendoMunka.Terulet = ValasztottTerulet;
            _szerkesztendoMunka.KezdoUzemora = KezdoUzemora;
            _szerkesztendoMunka.ZaroUzemora = ZaroUzemora;
            _szerkesztendoMunka.Kezdet = teljesKezdet;
            _szerkesztendoMunka.Veg = teljesVeg;
            await Shell.Current.DisplayAlert("Siker", "Munka módosítva!", "OK");
        }
        else
        {
            var ujMunka = new Munka
            {
                Tipus = MunkaTipus,
                Erogep = ValasztottGep,
                Munkaeszkoz = ValasztottEszkoz,
                Terulet = ValasztottTerulet,
                KezdoUzemora = KezdoUzemora,
                ZaroUzemora = ZaroUzemora,
                Kezdet = teljesKezdet,
                Veg = teljesVeg
            };
            _adatService.Munkak.Add(ujMunka);
            await Shell.Current.DisplayAlert("Siker", "Új munka mentve!", "OK");
        }

        _adatService.Mentes();
        await Shell.Current.GoToAsync("..");
    }
}