using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppAgrar.Models;
using MauiAppAgrar.Services;
using System.Collections.ObjectModel;

namespace MauiAppAgrar.ViewModels;

public partial class MunkaeszkozokViewModel : ObservableObject
{
    private readonly AdatService _adatService;

    [ObservableProperty] ObservableCollection<Munkaeszkoz> eszkozok;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VanKijeloles))]
    Munkaeszkoz kijeloltEszkoz;

    [ObservableProperty] string ujNev;
    [ObservableProperty] string ujKepUtvonal;

    public bool VanKijeloles => KijeloltEszkoz != null;

    public MunkaeszkozokViewModel(AdatService adatService)
    {
        _adatService = adatService;
        Eszkozok = new ObservableCollection<Munkaeszkoz>(_adatService.Eszkozok);
    }

    [RelayCommand]
    async Task Hozzaad()
    {
        if (string.IsNullOrWhiteSpace(UjNev)) return;

        var kep = string.IsNullOrEmpty(UjKepUtvonal) ? "eke.png" : UjKepUtvonal;
        var uj = new Munkaeszkoz { Nev = UjNev, KepUrl = kep };

        _adatService.Eszkozok.Add(uj);
        Eszkozok.Add(uj);

        _adatService.Mentes();

        UjNev = string.Empty;
        UjKepUtvonal = null;
        await Shell.Current.DisplayAlert("Siker", "Eszköz rögzítve", "OK");
    }

    [RelayCommand]
    void Torles()
    {
        if (KijeloltEszkoz == null) return;
        _adatService.Eszkozok.Remove(KijeloltEszkoz);
        Eszkozok.Remove(KijeloltEszkoz);

        _adatService.Mentes();

        KijeloltEszkoz = null;
    }

    [RelayCommand]
    async Task KepValasztas()
    {
        try
        {
            string action = await Shell.Current.DisplayActionSheet("Kép forrása", "Mégse", null, "Fényképezés", "Galéria");
            FileResult photo = null;
            if (action == "Fényképezés" && MediaPicker.Default.IsCaptureSupported) photo = await MediaPicker.Default.CapturePhotoAsync();
            else if (action == "Galéria") photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using var stream = await photo.OpenReadAsync();
                using var newStream = File.OpenWrite(newFile);
                await stream.CopyToAsync(newStream);
                UjKepUtvonal = newFile;
            }
        }
        catch {}
    }
}