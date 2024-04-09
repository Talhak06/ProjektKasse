using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjektKasse.Daten;
using ProjektKasse;


Produkt produkt_1 = new Produkt()
 {
     Name = "Chips",
     Preis = 1.50M 
};
dbContext.Produkte.Add(produkt_1);
await dbContext.SaveChangesAsync();


Produkt produkt_2 = new Produkt()
 {
     Name = "Kaugummi",
     Preis = 0.50M 
};
dbContext.Produkte.Add(produkt_2);
await dbContext.SaveChangesAsync();


Produkt produkt_3 = new Produkt()
 {
     Name = "Wasser",
     Preis = 1.00M 
};
dbContext.Produkte.Add(produkt_3);
await dbContext.SaveChangesAsync();


Produkt produkt_4 = new Produkt()
 {
     Name = "RedBull",
     Preis = 2.00M 
};
dbContext.Produkte.Add(produkt_4);
await dbContext.SaveChangesAsync();
