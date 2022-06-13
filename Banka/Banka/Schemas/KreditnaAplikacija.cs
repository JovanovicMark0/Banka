using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Schemas
{
    public class KreditnaAplikacija
    {
        public string nazivBanke { get; set; }
        public Klijent klijent { get; set; }
        public int iznosKredita { get; set; }
        public int brojMesecnihRata { get; set; }

        public KreditnaAplikacija(string nazivBanke, Klijent klijent , int iznosKredita, int brojMesecnihRata)
        {
            this.nazivBanke = nazivBanke;
            this.klijent = klijent;
            this.iznosKredita = iznosKredita;
            this.brojMesecnihRata = brojMesecnihRata;
        }

        public string ToString()
        {
            string tekst = "\t Banka : " + nazivBanke +
                            "\n\t Iznos kredita : " + iznosKredita +
                            "\n\t Broj mesečnih rata : " + brojMesecnihRata +
                            "\n\t JMBG klijenta : " + klijent.JMBG + ".\n";
            return tekst;
        }
    }
}
