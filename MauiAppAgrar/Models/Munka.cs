namespace MauiAppAgrar.Models;

public class Munka
{
    public string Tipus { get; set; }
    public Erogep Erogep { get; set; }
    public Munkaeszkoz Munkaeszkoz { get; set; }
    public Terulet Terulet { get; set; }

    public double KezdoUzemora { get; set; }
    public double ZaroUzemora { get; set; }

    public DateTime Kezdet { get; set; }
    public DateTime Veg { get; set; }

    public string DatumKijelzes
    {
        get
        {
            // Ha a nap megegyezik (év, hó, nap)
            if (Kezdet.Date == Veg.Date)
            {
                return $"{Kezdet:yyyy.MM.dd.}";
            }
            // Ha több napos a munka
            else
            {
                // pl.: 2023.10.27. - 10.29.
                return $"{Kezdet:yyyy.MM.dd.} - {Veg:MM.dd.}";
            }
        }
    }
}