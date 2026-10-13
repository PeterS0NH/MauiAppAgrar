using MauiAppAgrar.Models;
using System.Text.Json;

namespace MauiAppAgrar.Services;

public class AdatService
{
    public List<Erogep> Erogepek { get; } = new();
    public List<Munkaeszkoz> Eszkozok { get; } = new();
    public List<Terulet> Teruletek { get; } = new();
    public List<Munka> Munkak { get; } = new();

    private string _fajlUtvonal => Path.Combine(FileSystem.Current.AppDataDirectory, "adatok.json");

    public AdatService()
    {
        Betoltes();
    }

    public void Mentes()
    {
        try
        {
            var mentendoAdat = new MentettAdatok
            {
                Erogepek = Erogepek,
                Eszkozok = Eszkozok,
                Teruletek = Teruletek,
                Munkak = Munkak
            };
            string json = JsonSerializer.Serialize(mentendoAdat);
            File.WriteAllText(_fajlUtvonal, json);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Hiba a mentésnél: {ex.Message}");
        }
    }

    private void Betoltes()
    {
        try
        {
            if (File.Exists(_fajlUtvonal))
            {
                string json = File.ReadAllText(_fajlUtvonal);
                var betoltott = JsonSerializer.Deserialize<MentettAdatok>(json);

                if (betoltott != null)
                {
                    Erogepek.Clear();
                    Erogepek.AddRange(betoltott.Erogepek);

                    Eszkozok.Clear();
                    Eszkozok.AddRange(betoltott.Eszkozok);

                    Teruletek.Clear();
                    Teruletek.AddRange(betoltott.Teruletek);

                    Munkak.Clear();
                    if (betoltott.Munkak != null) Munkak.AddRange(betoltott.Munkak);
                }
            }
            else
            {
                MintaAdatokFeltoltese();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Hiba a betöltésnél: {ex.Message}");
            MintaAdatokFeltoltese();
        }
    }

    private void MintaAdatokFeltoltese()
    {
        if (Erogepek.Count == 0)
        {
            Erogepek.Add(new Erogep { Nev = "John Deere 8R" });
        }
        if (Eszkozok.Count == 0)
        {
            Eszkozok.Add(new Munkaeszkoz { Nev = "Eke" });
        }
        if (Teruletek.Count == 0)
        {
            Teruletek.Add(new Terulet { Nev = "Nagy tábla", NagysagHa = 50, Tulajdonos = "Saját" });
        }
    }

    public class MentettAdatok
    {
        public List<Erogep> Erogepek { get; set; } = new();
        public List<Munkaeszkoz> Eszkozok { get; set; } = new();
        public List<Terulet> Teruletek { get; set; } = new();
        public List<Munka> Munkak { get; set; } = new();
    }
}