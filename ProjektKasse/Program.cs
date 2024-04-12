using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektKasse.Daten;
using ProjektKasse.Model;






class Program
{
    static async Task Main(string[] args)
    {
        // Verbindung zur Datenbank herstellen
        using (var dbContext = new ProjektKasseContext())
        {
            while (true)
            {
                // Menü anzeigen
                Console.WriteLine("Menü:");
                Console.WriteLine("1. Produkte anzeigen");
                Console.WriteLine("2. Produkt hinzufügen");
                Console.WriteLine("3. Produkt löschen");
                Console.WriteLine("4. Produkt kaufen");
                Console.WriteLine("5. Umsätze anzeigen");
                Console.WriteLine("6. Beenden");
                Console.WriteLine("Wählen Sie eine Option:");

                // Benutzereingabe lesen
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1": // Produkte anzeigen
                        await ShowProducts(dbContext);
                        break;
                    case "2": // Produkt hinzufügen
                        await AddProduct(dbContext);
                        break;
                    case "3": // Produkt löschen
                        await DeleteProduct(dbContext);
                        break;
                    case "4": // Produkt kaufen
                        await BuyProducts(dbContext);
                        break;
                    case "5": // Umsätze anzeigen
                        await ShowSales(dbContext);
                        break;
                    case "6": // Beenden
                        return;
                    default:
                        Console.WriteLine("Ungültige Option!");
                        break;
                }
            }
        }
    }

    static async Task ShowProducts(ProjektKasseContext dbContext)
    {
        // Die Produkte aus der Datenbank abrufen und anzeigen
        List<Produkt> produkte = await dbContext.Produkte.ToListAsync();
        foreach (var produkt in produkte)
        {
            Console.WriteLine($"ID: {produkt.Id}, Name: {produkt.Name}, Preis: {produkt.Preis}");
        }
    }

    static async Task AddProduct(ProjektKasseContext dbContext)
    {
        // Benutzer um Namen und Preis des neuen Produkts bitten
        Console.WriteLine("Geben Sie den Namen des Produkts ein:");
        string name = Console.ReadLine();
        Console.WriteLine("Geben Sie den Preis des Produkts ein:");
        decimal preis;
        while (!decimal.TryParse(Console.ReadLine(), out preis))
        {
            Console.WriteLine("Ungültiger Preis. Bitte geben Sie eine gültige Dezimalzahl ein:");
        }

        // Ein neues Produkt erstellen und der Datenbank hinzufügen
        Produkt newProduct = new Produkt()
        {
            Name = name,
            Preis = preis
        };
        dbContext.Produkte.Add(newProduct);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Produkt wurde erfolgreich hinzugefügt!");
    }

    static async Task DeleteProduct(ProjektKasseContext dbContext)
    {
        // Benutzer um die ID des zu löschenden Produkts bitten
        Console.WriteLine("Geben Sie die ID des zu löschenden Produkts ein:");
        int productId;
        while (!int.TryParse(Console.ReadLine(), out productId))
        {
            Console.WriteLine("Ungültige ID. Bitte geben Sie eine gültige Ganzzahl ein:");
        }

        // Das Produkt anhand der ID finden und löschen
        Produkt productToDelete = await dbContext.Produkte.FirstOrDefaultAsync(p => p.Id == productId);
        if (productToDelete != null)
        {
            dbContext.Produkte.Remove(productToDelete);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Produkt wurde erfolgreich gelöscht!");
        }
        else
        {
            Console.WriteLine("Produkt mit dieser ID wurde nicht gefunden.");
        }
    }

    static async Task BuyProducts(ProjektKasseContext dbContext)
    {
        // Produkte anzeigen, um die Auswahl zu erleichtern
        await ShowProducts(dbContext);

        // Einen leeren Warenkorb erstellen
        List<Umsatz> warenkorb = new List<Umsatz>();

        while (true)
        {
            // Benutzer um Produkt-ID und Menge bitten
            Console.WriteLine("Geben Sie die ID des zu kaufenden Produkts ein (oder 'fertig', um den Kauf abzuschließen):");
            string input = Console.ReadLine();

            if (input.ToLower() == "fertig")
                break;

            if (!int.TryParse(input, out int productId))
            {
                Console.WriteLine("Ungültige Eingabe.");
                continue;
            }

            Console.WriteLine("Geben Sie die Menge des zu kaufenden Produkts ein:");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Ungültige Menge. Bitte geben Sie eine positive Ganzzahl ein:");
                continue;
            }

            // Das Produkt in der Datenbank finden
            Produkt productToBuy = await dbContext.Produkte.FirstOrDefaultAsync(p => p.Id == productId);
            if (productToBuy != null)
            {
                // Den Kauf zum Warenkorb hinzufügen
                Umsatz sale = new Umsatz()
                {
                    Gebucht = DateTime.Now,
                    ProduktId = productId,
                    Menge = quantity
                };
                warenkorb.Add(sale);
                Console.WriteLine($"Produkt '{productToBuy.Name}' mit Menge {quantity} zum Warenkorb hinzugefügt.");
            }
            else
            {
                Console.WriteLine("Produkt mit dieser ID wurde nicht gefunden.");
            }
        }

        // Alle Käufe im Warenkorb speichern
        dbContext.Umsätze.AddRange(warenkorb);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("Kauf erfolgreich!");
    }

    static async Task ShowSales(ProjektKasseContext dbContext)
    {
        // Umsätze aus der Datenbank abrufen und anzeigen
        List<Umsatz> umsätze = await dbContext.Umsätze.Include(u => u.Produkt).ToListAsync();
        foreach (var umsatz in umsätze)
        {
            Console.WriteLine($"ID: {umsatz.Id}, Datum: {umsatz.Gebucht}, Produkt: {umsatz.Produkt.Name}, Preis pro Stück: {umsatz.Produkt.Preis}, Menge: {umsatz.Menge}, Gesamtpreis: {umsatz.Produkt.Preis * umsatz.Menge}");
        }
    }
}



//class Program
//{
//    static async Task Main(string[] args)
//    {
//        // Verbindung zur Datenbank herstellen
//        using (var dbContext = new ProjektKasseContext())
//        {
//            while (true)
//            {
//                // Menü anzeigen
//                Console.WriteLine("Menü:");
//                Console.WriteLine("1. Produkte anzeigen");
//                Console.WriteLine("2. Produkt hinzufügen");
//                Console.WriteLine("3. Produkt löschen");
//                Console.WriteLine("4. Beenden");
//                Console.WriteLine("Wählen Sie eine Option:");

//                // Benutzereingabe lesen
//                string option = Console.ReadLine();

//                switch (option)
//                {
//                    case "1": // Produkte anzeigen
//                        await ShowProducts(dbContext);
//                        break;
//                    case "2": // Produkt hinzufügen
//                        await AddProduct(dbContext);
//                        break;
//                    case "3": // Produkt löschen
//                        await DeleteProduct(dbContext);
//                        break;
//                    case "4": // Beenden
//                        return;
//                    default:
//                        Console.WriteLine("Ungültige Option!");
//                        break;
//                }
//            }
//        }
//    }

//    static async Task ShowProducts(ProjektKasseContext dbContext)
//    {
//        // Die Produkte aus der Datenbank abrufen und anzeigen
//        List<Produkt> produkte = await dbContext.Produkte.ToListAsync();
//        foreach (var produkt in produkte)
//        {
//            Console.WriteLine($"ID: {produkt.Id}, Name: {produkt.Name}, Price: {produkt.Preis}");
//        }
//    }

//    static async Task AddProduct(ProjektKasseContext dbContext)
//    {
//        // Benutzer um Namen und Preis des neuen Produkts bitten
//        Console.WriteLine("Geben Sie den Namen des Produkts ein:");
//        string name = Console.ReadLine();
//        Console.WriteLine("Geben Sie den Preis des Produkts ein:");
//        decimal preis;
//        while (!decimal.TryParse(Console.ReadLine(), out preis))
//        {
//            Console.WriteLine("Ungültiger Preis. Bitte geben Sie eine gültige Dezimalzahl ein:");
//        }

//        // Ein neues Produkt erstellen und der Datenbank hinzufügen
//        Produkt newProduct = new Produkt()
//        {
//            Name = name,
//            Preis = preis
//        };
//        dbContext.Produkte.Add(newProduct);
//        await dbContext.SaveChangesAsync();
//        Console.WriteLine("Produkt wurde erfolgreich hinzugefügt!");
//    }

//    static async Task DeleteProduct(ProjektKasseContext dbContext)
//    {
//        // Benutzer um die ID des zu löschenden Produkts bitten
//        Console.WriteLine("Geben Sie die ID des zu löschenden Produkts ein:");
//        int productId;
//        while (!int.TryParse(Console.ReadLine(), out productId))
//        {
//            Console.WriteLine("Ungültige ID. Bitte geben Sie eine gültige Ganzzahl ein:");
//        }

//        // Das Produkt anhand der ID finden und löschen
//        Produkt productToDelete = await dbContext.Produkte.FirstOrDefaultAsync(p => p.Id == productId);
//        if (productToDelete != null)
//        {
//            dbContext.Produkte.Remove(productToDelete);
//            await dbContext.SaveChangesAsync();
//            Console.WriteLine("Produkt wurde erfolgreich gelöscht!");
//        }
//        else
//        {
//            Console.WriteLine("Produkt mit dieser ID wurde nicht gefunden.");
//        }
//    }
//}
//ProjektKasseContext dbContext = new ProjektKasseContext();

////Produkt produkt_1 = new Produkt()
////{
////    Name = "Chips",
////    Preis = 1.50M
////};
////dbContext.Produkte.Add(produkt_1);
////await dbContext.SaveChangesAsync();


////Produkt produkt_2 = new Produkt()
////{
////    Name = "Kaugummi",
////    Preis = 0.50M
////};
////dbContext.Produkte.Add(produkt_2);
////await dbContext.SaveChangesAsync();


////Produkt produkt_3 = new Produkt()
////{
////    Name = "Wasser",
////    Preis = 1.00M
////};
////dbContext.Produkte.Add(produkt_3);
////await dbContext.SaveChangesAsync();


////Produkt produkt_4 = new Produkt()
////{
////    Name = "RedBull",
////    Preis = 2.00M
////};
////dbContext.Produkte.Add(produkt_4);
////await dbContext.SaveChangesAsync();

//List<Produkt> produkte = await dbContext.Produkte.ToListAsync();



////Produkt produktToDelete = await dbContext.Produkte.FirstOrDefaultAsync(p => p.Id == 1); // Angenommen, die ID ist bekannt
////if (produktToDelete != null)
////{
////    dbContext.Produkte.Remove(produktToDelete);
////    await dbContext.SaveChangesAsync();
////}



//// Iterate over each Produkt and print its details
//foreach (var produkt in produkte)
//{
//    Console.WriteLine($"ID: {produkt.Id}, Name: {produkt.Name}, Price: {produkt.Preis}");
//    // Add other properties as needed
//}


