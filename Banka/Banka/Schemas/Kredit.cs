using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Schemas
{
    public class Kredit
    {
        public string status { get; set; }
        public string nazivBanke { get; set; }
        public string JMBG { get; set; }
        public int ukupnoRata { get; set; }
        public double iznosRate { get; set; }

        public Kredit(string status, string nazivBanke, string JMBG, int ukupnoRata, double iznosRate)
        {
            this.status = status;
            this.nazivBanke = nazivBanke;
            this.JMBG = JMBG;
            this.ukupnoRata = ukupnoRata;
            this.iznosRate = iznosRate;
        }

        public string ToString()
        {
            string tekst = "\n\n Status kredita : " + status + 
                            "\n\t Banka : " + nazivBanke + 
                            "\n\t Preostalo mesečnih rata : " + ukupnoRata + 
                            "\n\t Iznos mesečne rate : " + iznosRate +"\n";
            return tekst;
        }
    }
}
