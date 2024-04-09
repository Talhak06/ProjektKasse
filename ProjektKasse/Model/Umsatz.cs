using System;
using System.Collections.Generic;
using ProjektKasse.Model;

namespace ProjektKasse
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ProjktKasseContext())
            {
                // Produkte automatisch hinzufügen
                AddDefaultProdukte(context).Wait();

                var kasse = new Kasse(context);

                while (true)
                {
                    Console.WriteLine("Willkommen im Kassensystem. Bitte wählen Sie eine Option:");
                    Console.WriteLine("1. Produkt kaufen");
                    Console.WriteLine("2. Verlassen");

                    string eingabe = Console.ReadLine();

                    switch (eingabe)
                    {
                        case "1":
                            ProduktKaufen(kasse);
                            break;
                        case "2":
                            Console.WriteLine("Auf Wiedersehen!");
                            return;
                        default:
                            Console.WriteLine("Ungültige Eingabe. Bitte wählen Sie eine der verfügbaren Optionen.");
                            break;
                    }
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

        static void ProduktKaufen(Kasse kasse)
        {
            Console.WriteLine("Bitte geben Sie den Namen des gekauften Produkts ein:");
            string produktName = Console.ReadLine();

            Console.WriteLine("Bitte geben Sie den Preis des gekauften Produkts ein:");
            decimal preis;
            while (!decimal.TryParse(Console.ReadLine(), out preis) || preis <= 0)
            {
                Console.WriteLine("Ungültiger Preis. Bitte geben Sie eine positive Dezimalzahl ein:");
            }

            var produkt = new Produkt { Name = produktName, Preis = preis };
            kasse.Kaufen(new List<Produkt> { produkt }).Wait();

            Console.WriteLine("Produkt erfolgreich gekauft.");
        }
    }
}
