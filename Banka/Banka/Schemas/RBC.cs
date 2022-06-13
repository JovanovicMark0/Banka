using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Schemas
{
    public class RBC : Banka
    {
        public RBC() 
        {
            naziv = "RBC";
            odnosPlateIRate = 0.5;
        }

        public override void obradiAplikacije(string JMBG) 
        {
            bool prolaziUsloveBanke = true;
            //Kreditne aplikacije korisnika za Banku RBC
            List<KreditnaAplikacija> kreditneAplikacijeList = iscitajKreditneAplikacije(JMBG);

            if (kreditneAplikacijeList.Count() > 0)
            {
                prolaziUsloveBanke = nemaAktivanKredit(JMBG);

                //Provera da li korisnik ima duplo veću platu od rate
                if (prolaziUsloveBanke)
                {
                    bool odobren_kredit = false;
                    foreach(var kreditnaAplikacija in kreditneAplikacijeList)
                    {
                        if(odnosPlateIRateCalculator(kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata))
                        {
                            odobren_kredit = upisiKredit(new Kredit("aktivan", kreditnaAplikacija.nazivBanke, kreditnaAplikacija.klijent.JMBG , kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita / kreditnaAplikacija.brojMesecnihRata), null);
                            if(odobren_kredit)
                            {
                                Console.WriteLine("Korisniku sa JMBG-om : {0} je odobren kredit u iznosu od {1} na {2} rata.\n", kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata);
                                obrisiAplikaciju(kreditnaAplikacija);
                            }
                            break;
                        }
                    }
                    if(!odobren_kredit)
                    {
                        Console.WriteLine("Sve aplikacije u banci {0} za korisnika {1} su odbijene.\n", naziv, JMBG);
                    }
                }
            }
            else
            {
                Console.WriteLine("Ne postoje kreditne aplikacije korisnika sa JMBG-om {0}, u banci {1}. \n", JMBG, naziv);
            }
        }

        public override void obradiAplikaciju(KreditnaAplikacija kreditnaAplikacija)
        {
            bool prolaziUsloveBanke = nemaAktivanKredit(kreditnaAplikacija.klijent.JMBG);
            if (prolaziUsloveBanke && odnosPlateIRateCalculator(kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita, kreditnaAplikacija.brojMesecnihRata))
            {
                if(!upisiKredit(new Kredit("aktivan", kreditnaAplikacija.nazivBanke, kreditnaAplikacija.klijent.JMBG, kreditnaAplikacija.brojMesecnihRata, kreditnaAplikacija.iznosKredita / kreditnaAplikacija.brojMesecnihRata), null))
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

        public bool nemaAktivanKredit(string JMBG)
        {
            bool prolaziUsloveBanke = true;
            List<Kredit> kreditiList = izlistajKredite(JMBG, naziv, null);

            //Provera da li postoji aktivan kredit za korisnika
            if (kreditiList != null)
            {
                foreach (var kredit in kreditiList)
                {
                    if (kredit.status == "aktivan")
                    {
                        prolaziUsloveBanke = false;
                        break;
                    }
                }
            }

            return prolaziUsloveBanke;
        }
    }
}
