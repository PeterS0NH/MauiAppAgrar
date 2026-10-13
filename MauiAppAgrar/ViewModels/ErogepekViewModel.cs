using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiAppAgrar.Models;
using MauiAppAgrar.Services;
using System.Collections.ObjectModel;

namespace MauiAppAgrar.ViewModels;

public partial class ErogepekViewModel : ObservableObject
{
    private readonly AdatService _adatService;

    [ObservableProperty] ObservableCollection<Erogep> erogepek;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(VanKijeloles))]
    Erogep kijeloltGep;

    [ObservableProperty] string ujNev;
    [ObservableProperty] string ujKepUtvonal;

    public bool VanKijeloles => KijeloltGep != null;

    public ErogepekViewModel(AdatService adatService)
    {
        _adatService = adatService;
        Erogepek = new ObservableCollection<Erogep>(_adatService.Erogepek);
    }

    [RelayCommand]
    async Task Hozzaad()
    {
        if (string.IsNullOrWhiteSpace(UjNev)) return;

        var kep = string.IsNullOrEmpty(UjKepUtvonal) ? "traktor_ikon.png" : UjKepUtvonal;

        var uj = new Erogep { Nev = UjNev, KepUrl = kep };
        _adatService.Erogepek.Add(uj);
        Erogepek.Add(uj);

        _adatService.Mentes();

        UjNev = string.Empty;
        UjKepUtvonal = null;
        await Shell.Current.DisplayAlert("Siker", "Erőgép rögzítve", "OK");
    }

    [RelayCommand]
    void Torles()
    {
        if (KijeloltGep == null) return;

        _adatService.Erogepek.Remove(KijeloltGep);
        Erogepek.Remove(KijeloltGep);

        _adatService.Mentes();

        KijeloltGep = null;
    }

    [RelayCommand]
    async Task KepValasztas()
    {
        try
        {
            string action = await Shell.Current.DisplayActionSheet("Kép forrása", "Mégse", null, "Fényképezés", "Galéria");
            FileResult photo = null;

            if (action == "Fényképezés")
            {
                if (MediaPicker.Default.IsCaptureSupported)
                    photo = await MediaPicker.Default.CapturePhotoAsync();
            }
            else if (action == "Galéria")
            {
                photo = await MediaPicker.Default.PickPhotoAsync();
            }

            if (photo != null)
            {
                var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using (var stream = await photo.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);

                UjKepUtvonal = newFile;
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hiba", $"Nem sikerült a képet betölteni: {ex.Message}", "OK");
        }
    }
}