using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Schemas
{
    public class Wells_Fargo : Banka
    {
        public Wells_Fargo()
        {
            naziv = "Wells Fargo";
            odnosPlateIRate = 1;
        }

        public override void obradiAplikacije(string JMBG)
        {
            //Kreditne aplikacije korisnika za Banku Wells Fargo
            List<KreditnaAplikacija> kreditneAplikacijeList = iscitajKreditneAplikacije(JMBG);
            if (kreditneAplikacijeList.Count() > 0)
            {
                bool odobren_kredit = false;
                foreach (var kreditnaAplikacija in kreditneAplikacijeList)
                {
                    if (odnosPlateIRateCalculator(kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata))
                    {
                        odobren_kredit = upisiKredit(new Kredit("aktivan", kreditnaAplikacija.nazivBanke, kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita / kreditnaAplikacija.brojMesecnihRata), null);
                        if (odobren_kredit)
                        {
                            Console.WriteLine("Korisniku sa JMBG-om : {0} je odobren kredit u iznosu od {1} na {2} rata.\n", kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata);
                            obrisiAplikaciju(kreditnaAplikacija);
                        }
                        break;
                    }
                }
                if (!odobren_kredit)
                {
                    Console.WriteLine("Sve aplikacije u banci {0} za korisnika {1} su odbijene.\n", naziv, JMBG);
                }
            }
        }

        public override void obradiAplikaciju(KreditnaAplikacija kreditnaAplikacija)
        {
            if (odnosPlateIRateCalculator(kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata))
            {
                if (!upisiKredit(new Kredit("aktivan", kreditnaAplikacija.nazivBanke, kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita / kreditnaAplikacija.brojMesecnihRata), null))
                {
                    Console.WriteLine("Kreditna aplikacija je odbijena. \n {0}", kreditnaAplikacija.ToString());
                }
                else
                {
                    Console.WriteLine("Korisniku sa JMBG-om : {0} je odobren kredit u iznosu od {1} na {2} rata.\n", kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata);
                    obrisiAplikaciju(kreditnaAplikacija);
                }
            }
            else
            {
                Console.WriteLine("Kreditna aplikacija je odbijena. \n {0}", kreditnaAplikacija.ToString());
            }
        } 
    }
}
