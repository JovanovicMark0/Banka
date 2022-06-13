using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Banka.Schemas;

namespace Banka.Schemas
{
    public abstract class Banka
    {
        public string naziv { get; set; }

        //svaka banka ima određeni odnos mesečnih primanja i rate za kredit
        //koji mora da se ispuni kako bi se odobrio kredit
        //RBC         rata < 50% plate   -> trazeniOdnos = 0.5
        //Santander   rata < 70% plate   -> trazeniOdnos = 0.7
        //Wells Fargo rata < 100% plate  -> trazeniOdnos = 1
        public double odnosPlateIRate { get; set; } 
        public string ToString()
        {
            string tekst = "Banka : " + naziv;
            return tekst;
        }

        public List<KreditnaAplikacija> iscitajKreditneAplikacije(string JMBG)
        {
            string csvPath = @"kreditne_aplikacije.csv";
            List<KreditnaAplikacija> kreditneAplikacijeList = new List<KreditnaAplikacija>();

            //Čitanje kreditnih aplikacija
            try
            {
                using (var reader = new StreamReader(csvPath))
                {
                    KreditnaAplikacija kreditnaAplikacija;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if(values[0].Equals(naziv))
                        {
                            if (JMBG == null || values[1].Equals(JMBG))
                            { 
                                kreditnaAplikacija = new KreditnaAplikacija(values[0], new Klijent(values[1], values[2], Int32.Parse(values[3]), Int32.Parse(values[4])), Int32.Parse(values[5]), Int32.Parse(values[6]));
                                kreditneAplikacijeList.Add(kreditnaAplikacija);
                            }
                        }
                    }
                }
                return kreditneAplikacijeList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGreška prilikom učitavanje kreditnih aplikacija! \n {0}", ex.Message);
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
            return null;
        }

        public List<Kredit> izlistajKredite(string JMBG, string imeBanke, string? status)
        {
            List<Kredit> kreditiList = new List<Kredit>();
            //Čitanje kredita
            try
            {
                using (var reader = new StreamReader(@"krediti.csv"))
                {
                    Kredit kredit;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (values.Length == 5 && values[1].Equals(imeBanke) && JMBG.Equals(values[2]))
                        {
                            if (status == null || values[0].Equals(status))
                            {
                                kredit = new Kredit(values[0], values[1], values[2], Int32.Parse(values[3]), Double.Parse(values[4]));
                                kreditiList.Add(kredit);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGreška prilikom učitavanja kredita! \n {0}", ex.Message);
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            }
            return kreditiList;
        }

        //Računanje iznosa mesečne rate, traženi odnos je u %
        //RBC         rata < 50% plate   -> trazeniOdnos = 0.5
        //Santander   rata < 70% plate   -> trazeniOdnos = 0.7
        //Wells Fargo rata < 100% plate  -> trazeniOdnos = 1
        public bool odnosPlateIRateCalculator(int mesecnaPrimanja, int trazeniKredit, int brojRata)
        {
            double rata = trazeniKredit / brojRata;
            if (mesecnaPrimanja * odnosPlateIRate > rata)
                return true;
            else
                return false;
        }

        public bool upisiKredit(Kredit kredit, string? putanja)
        {
            //Dodavanje kreditne aplikacije u csv fajl
            try
            {
                string csvPath;
                if(putanja == null)
                    csvPath = @"krediti.csv";
                else
                    csvPath = putanja;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(csvPath, true))
                {
                    file.WriteLine(
                                    kredit.status + "," +
                                    kredit.nazivBanke + "," +
                                    kredit.JMBG + "," +
                                    kredit.ukupnoRata + "," +
                                    kredit.iznosRate );
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGreška prilikom odobravanja kredita! \n {0} \n", ex.Message);
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                return false;
            }
        }

        public bool dodajKreditnuAplikaciju(KreditnaAplikacija kreditnaAplikacija, string? csvPutanja)
        {
            //Dodavanje kreditne aplikacije u csv fajl
            try
            {
                string csvPath;
                if (csvPutanja == null)
                    csvPath = @"kreditne_aplikacije.csv";
                else
                    csvPath = csvPutanja;

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(csvPath, true))
                {
                    file.WriteLine(
                                    kreditnaAplikacija.nazivBanke + "," +
                                    kreditnaAplikacija.klijent.ime + "," +
                                    kreditnaAplikacija.klijent.JMBG + "," +
                                    kreditnaAplikacija.klijent.mesecnaPrimanja + "," +
                                    kreditnaAplikacija.klijent.godineRadnogStaza + "," +
                                    kreditnaAplikacija.iznosKredita + "," +
                                    kreditnaAplikacija.brojMesecnihRata 
                                    );
                }
                Console.WriteLine("\nDodata kreditna aplikacija : \n" + kreditnaAplikacija.ToString());
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                obradiAplikaciju(kreditnaAplikacija);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nGreška prilikom dodavanja kreditne aplikacije! \n {0} \n", ex.Message);
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                return false;
            }
        }

        public void obrisiAplikaciju(KreditnaAplikacija kreditnaAplikacija) 
        {
            String tempFile = "tempA.csv";
            KreditnaAplikacija kreditnaAplikacijaFajl = null;
            try
            {
                using (var reader = new StreamReader(@"kreditne_aplikacije.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(",");

                        kreditnaAplikacijaFajl = new KreditnaAplikacija(values[0], new Klijent(values[1], values[2], Int32.Parse(values[3]), Int32.Parse(values[4])), Int32.Parse(values[5]), Int32.Parse(values[6]));

                        if (!kreditnaAplikacijaFajl.nazivBanke.Equals(kreditnaAplikacija.nazivBanke) || !kreditnaAplikacijaFajl.klijent.JMBG.Equals(kreditnaAplikacija.klijent.JMBG) || kreditnaAplikacijaFajl.iznosKredita != kreditnaAplikacija.iznosKredita || kreditnaAplikacijaFajl.brojMesecnihRata != kreditnaAplikacija.brojMesecnihRata)
                        {
                            dodajKreditnuAplikaciju(kreditnaAplikacijaFajl, tempFile);
                        }
                    }
                }

                File.Delete("kreditne_aplikacije.csv");

                System.IO.File.Move(tempFile, "kreditne_aplikacije.csv");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nNeuspešno brisanje aplikacije! \n" + ex.Message);
            }
        }

        public abstract void obradiAplikacije(string JMBG);
        public abstract void obradiAplikaciju(KreditnaAplikacija kreditnaAplikacija);
        public bool uplatiRatuKredita(Kredit kredit)
        {
            bool isplacenKredit = false;
            String tempFile = "temp.csv";
            Kredit kreditFajl = null;
            try
            {
                using (var reader = new StreamReader(@"krediti.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(",");

                        kreditFajl = new Kredit(values[0], values[1], values[2], Int32.Parse(values[3]), Double.Parse(values[4]));

                        if(kreditFajl.status.Equals(kredit.status) && kreditFajl.JMBG.Equals(kredit.JMBG) && kreditFajl.nazivBanke.Equals(kredit.nazivBanke) && kreditFajl.iznosRate == kredit.iznosRate && kreditFajl.ukupnoRata == kredit.ukupnoRata)
                        {
                            kreditFajl.ukupnoRata--;
                            if (kreditFajl.ukupnoRata == 0)
                            {
                                isplacenKredit = true;
                                kreditFajl.status = "neaktivan";
                            }
                        }

                        upisiKredit(kreditFajl, tempFile);
                    }
                }

                File.Delete("krediti.csv");

                System.IO.File.Move(tempFile, "krediti.csv");
                if (kreditFajl != null && !isplacenKredit)
                {
                    Console.WriteLine("\nUspešno ste platili ratu kredita! \nPreostalo Vam je još {0} rata.", kreditFajl.ukupnoRata);
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                }
                else if (kreditFajl != null && isplacenKredit)
                {
                    Console.WriteLine("\nUspešno ste isplatili kredit.\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                }
                else
                {
                    Console.WriteLine("\nUplata rate za kredit neuspela!\n");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nUplata rate za kredit neuspela! \n" + ex.Message);
                return false;
            }
        }
    }
}
