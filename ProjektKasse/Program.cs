using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjektKasse.Model;

namespace ProjektKasse
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var context = new ProjktKasseContext())
            {
                // Produkte automatisch hinzufügen
                await AddDefaultProdukte(context);

                var kasse = new Kasse(context);

                while (true)
                {
                    Console.WriteLine("Willkommen im Kassensystem. Bitte wählen Sie die Produkte aus:");
                    Console.WriteLine("0. Beenden");

                    var produkte = context.Produkte.ToList();
                    for (int i = 0; i < produkte.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {produkte[i].Name} - {produkte[i].Preis:C}");
                    }

                    Console.WriteLine("Wählen Sie die Nummer des Produkts (oder 0 zum Beenden):");
                    string eingabe = Console.ReadLine();

                    if (eingabe == "0")
                    {
                        Console.WriteLine("Auf Wiedersehen!");
                        return;
                    }

                    int produktIndex;
                    if (!int.TryParse(eingabe, out produktIndex) || produktIndex < 1 || produktIndex > produkte.Count)
                    {
                        Console.WriteLine("Ungültige Auswahl. Bitte geben Sie die Nummer des Produkts ein.");
                        continue;
                    }

                    var gekauftesProdukt = produkte[produktIndex - 1];
                    await kasse.Kaufen(new List<Produkt> { gekauftesProdukt });

                    Console.WriteLine($"{gekauftesProdukt.Name} erfolgreich gekauft.");

                    Console.WriteLine("Möchten Sie ein weiteres Produkt kaufen? (ja/nein)");
                    eingabe = Console.ReadLine();

                    if (eingabe.ToLower() != "ja")
                        break;
                }
            }
        }

        static async Task AddDefaultProdukte(ProjktKasseContext context)
        {
            var produkte = new List<Produkt>
            {
                new Produkt { Name = "Chips", Preis = 1.50M },
                new Produkt { Name = "Kaugummi", Preis = 0.50M },
                new Produkt { Name = "Wasser", Preis = 1.00M },
                new Produkt { Name = "RedBull", Preis = 2.00M }
            };

            context.AddRange(produkte);
            await context.SaveChangesAsync();
        }
    }
}
