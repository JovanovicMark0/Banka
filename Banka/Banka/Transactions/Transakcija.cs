using Banka.Schemas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Transactions
{
    public static class Transakcija
    {
        //UPLATA MESEČNE RATE KREDITA
        public static void uplataRate()
        {
        odabirOpcija2:
            Console.WriteLine("\n Kako bi uplatili ratu molimo unesite sledeće podatke u navedenom redosledu, za kraj unosa pritisnite Enter:\n" +
                               "\t 1) JMBG \n" +
                               "\t 2) Naziv banke \n");
            
            List<Kredit> kreditiList = null;
            Banka.Schemas.Banka banka = null;
            using (var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
            {
                var input = sr.ReadLine();

                var tokens = input.Replace(Environment.NewLine, "\n").Split(" ");
                if (tokens[0] != null && tokens[0].Equals("0"))
                {
                    Console.WriteLine("\n Hvala Vam na saradnji, prijatno!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    Environment.Exit(0);
                }

                if (tokens.Length == 2)
                {
                    if (tokens[0].Length == 10)
                    {
                        BankaFactory bankaFactory = new BankaFactory();
                        banka = bankaFactory.makeBanka(tokens[1]);
                        if (banka == null)
                        {
                            Console.WriteLine("Poštovani pogrešno ste uneli naziv banke! (RBC/Santander/Wells Fargo)\n");
                            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                            goto odabirOpcija2;
                        }
                        kreditiList = banka.izlistajKredite(tokens[0], banka.naziv, "aktivan");
                    }
                    else
                    {
                        Console.WriteLine("Poštovani JMBG mora imati 10 karaktera!\n");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                        goto odabirOpcija2;
                    }
                }
                else
                {
                    Console.WriteLine("Poštovani niste uneli sve potrebne podatke!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

                    goto odabirOpcija2;
                }
            }

            //Ispis svih kredita koje korisnik ima u datoj banci i odabir kredita
            if (kreditiList.Count() > 0)
            {
                Console.Write("\n Unesite redni broj opcije za koji želite uplatiti ratu  : \n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                int i = 1;
                foreach (Kredit kredit in kreditiList)
                {
                    Console.WriteLine("\n Opcija {0}. : \n" + kredit.ToString(), i);
                    i++;
                }

                int izabranaOpcija = Convert.ToInt32(Console.ReadLine());
                while (izabranaOpcija < 0 || izabranaOpcija > kreditiList.Count())
                {
                    Console.WriteLine("\nOdabrali ste nepostojeću opciju, molimo pokušajte ponovo.\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    izabranaOpcija = Convert.ToInt32(Console.ReadLine());
                }
                if (banka != null)
                {
                    banka.uplatiRatuKredita(kreditiList[izabranaOpcija - 1]);
                }
            }
            else
            {
                Console.Write("\nPoštovani u banci {0} nemate aktivnih kredita.\n", banka.naziv);
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
        }


        //PRIKAZ INFORMACIJA O KREDITIMA KORISNIKA
        public static void prikazInformacijaOKreditima()
        {
        odabirOpcija3:
            Console.WriteLine("\n Kako bi dobili informacije o kreditima molimo unesite sledeće podatke u navedenom redosledu, za kraj unosa pritisnite Enter:\n" +
                               "\t 1) JMBG \n" +
                               "\t 2) Naziv banke  \n");

            using (var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
            {
                var input = sr.ReadLine();

                var tokens = input.Replace(Environment.NewLine, "\n").Split(" ");

                if (tokens[0] != null && tokens[0].Equals("0"))
                {
                    Console.WriteLine("\n Hvala Vam na saradnji, prijatno!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    Environment.Exit(0);
                }

                if (tokens.Length == 2)
                {
                    if (tokens[0].Length == 10)
                    {
                        BankaFactory bankaFactory = new BankaFactory();
                        Banka.Schemas.Banka banka = bankaFactory.makeBanka(tokens[1]);
                        if (banka == null)
                        {
                            Console.WriteLine("Poštovani pogrešno ste uneli naziv banke! (RBC/Santander/Wells Fargo)");
                            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                            goto odabirOpcija3;
                        }

                        List<Kredit> kreditiList = banka.izlistajKredite(tokens[0], banka.naziv, null);
                        if(kreditiList.Count() == 0)
                        {
                            Console.WriteLine("\nPoštovani trenutno nemate kredit u banci {0}.\n", banka.naziv);
                            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                        }
                        foreach (Kredit kredit in kreditiList)
                        {
                            Console.Write(kredit.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("Poštovani JMBG mora imati 10 karaktera!\n");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                        goto odabirOpcija3;
                    }
                }
                else
                {
                    Console.WriteLine("Poštovani niste uneli sve potrebne podatke!");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    goto odabirOpcija3;
                }
            }
        }


        //KORISNIK UNOSI APLIKACIJE IZ FAJLA
        public static void noveAplikacije()
        {
        odabirOpcija4:
            Console.WriteLine("\n Kako bi uneli nove aplikacije molimo unesite putanju do fajla.");

            using (var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
            {
                var input = sr.ReadLine();

                var tokens = input.Replace(Environment.NewLine, "\n").Split(" ");

                if (tokens[0] != null && tokens[0].Equals("0"))
                { 
                    Console.WriteLine("\n Hvala Vam na saradnji, prijatno!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    Environment.Exit(0);
                }
                

                if (tokens.Length == 1)
                {
                    string csvPath = tokens[0];
                    List<KreditnaAplikacija> kreditneAplikacijeList = new List<KreditnaAplikacija>();
                    try
                    {
                        //Čitanje kreditnih aplikacija iz fajla
                        using (var reader = new StreamReader(csvPath))
                        {
                            KreditnaAplikacija kreditnaAplikacija;
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');

                                kreditnaAplikacija = new KreditnaAplikacija(values[0], new Klijent(values[1], values[2], Int32.Parse(values[3]), Int32.Parse(values[4])), Int32.Parse(values[5]), Int32.Parse(values[6]));
                                kreditneAplikacijeList.Add(kreditnaAplikacija);
                            }
                        }

                        BankaFactory bankaFactory = new BankaFactory();
                        Banka.Schemas.Banka banka = null;
                        //Upisivanje kreditnih aplikacija u fajl
                        foreach (KreditnaAplikacija kreditnaAplikacija in kreditneAplikacijeList)
                        {
                            banka = bankaFactory.makeBanka(kreditnaAplikacija.nazivBanke);
                            banka.dodajKreditnuAplikaciju(kreditnaAplikacija, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Greška prilikom dodavanja kreditnih aplikacija iz fajla! \n {0}", ex.Message);
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    }
                }
                else
                {
                    Console.WriteLine("Poštovani niste uneli tačne podatke!");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    goto odabirOpcija4;
                }
            }
        }


        //KORISNIK PODNOSI APLIKACIJU ZA KREDIT
        public static void zahtevZaKredit()
        {
        odabirOpcija5:
            KreditnaAplikacija kreditnaAplikacija = null;
            Console.WriteLine("\n Kako bi aplicirali za kredit molimo unesite sledeće podatke u navedenom redosledu, za kraj unosa pritisnite Enter:\n" +
                               "\t 1) Naziv banke \n" +
                               "\t 2) Ime \n" +
                               "\t 3) JMBG \n" +
                               "\t 4) Mesečna primanja \n" +
                               "\t 5) Godine radnog staža \n" +
                               "\t 6) Iznos kredita \n" +
                               "\t 7) Broj mesečnih rata \n");

            using (var sr = new StreamReader(Console.OpenStandardInput(), Console.InputEncoding))
            {
                var input = sr.ReadLine();

                var tokens = input.Replace(Environment.NewLine, "\n").Split(" ");
                if (tokens[0] != null && tokens[0].Equals("0"))
                {
                    Console.WriteLine("\n Hvala Vam na saradnji, prijatno!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    Environment.Exit(0);
                }

                if (tokens.Length == 7)
                {
                    if (tokens[2].Length == 10)
                    {
                        kreditnaAplikacija = new KreditnaAplikacija(tokens[0], new Klijent(tokens[1], tokens[2], Int32.Parse(tokens[3]), Int32.Parse(tokens[4])), Int32.Parse(tokens[5]), Int32.Parse(tokens[6]));
                        BankaFactory bankaFactory = new BankaFactory();
                        Banka.Schemas.Banka banka = bankaFactory.makeBanka(kreditnaAplikacija.nazivBanke);
                        if (banka != null)
                        {
                            banka.dodajKreditnuAplikaciju(kreditnaAplikacija, null);
                        }
                        else
                        {
                            Console.WriteLine("Poštovani došlo je do greške, molimo pokušajte ponovo.");
                            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                            goto odabirOpcija5;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Poštovani JMBG mora imati 10 karaktera!\n");
                        Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                        goto odabirOpcija5;
                    }
                }
                else
                {
                    Console.WriteLine("Poštovani niste uneli sve potrebne podatke!");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                    goto odabirOpcija5;
                }
            }
        }
    }
}
