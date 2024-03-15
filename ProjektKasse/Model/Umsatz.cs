using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektKasse.Model
{
    internal class Umsatz
    {
        public int Id { get; set; }
        public DateTime Gebucht { get; set; }
        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; } = null!;
    }
}
