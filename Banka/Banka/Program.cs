using Banka.Schemas;
using Banka.Transactions;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Banka
{
    class Program
    {
        static void Main(string[] args)
        {
        odabirOpcija1:
            Console.WriteLine("Odaberite opciju: (Ukoliko želite da napustite program u bilo kom trenutku unesite samo 0)\n"+
                                "\t 1) Uplata rate. \n" +
                                "\t 2) Prikaz informacija o kreditima. \n" +
                                "\t 3) Nove aplikacije. \n"+
                                "\t 4) Aplikacija za kredit. \n" );
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            string m = Console.ReadLine();
            
            switch (m)
            {
                case "1":
                    Transakcija.uplataRate();
                    break;

                case "2":
                    Transakcija.prikazInformacijaOKreditima();
                    break;
                case "3":
                    Transakcija.noveAplikacije();
                    break;
                case "4":
                    Transakcija.zahtevZaKredit();
                    break;
                case "0":
                    Console.WriteLine("\n Hvala Vam na saradnji, prijatno!\n");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nNepostojeći izbor! (Izbor 1-4)\n");
                    goto odabirOpcija1;
            }
            Console.WriteLine("\n\n\tŽelite li da obavite još neku transakciju? D/N\n");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            string odgovor = Console.ReadLine();
            while(!odgovor.Equals("D") && !odgovor.Equals("N"))
            {
                Console.WriteLine("\nMolimo odgovorite sa D ili N.\n");
                odgovor = Console.ReadLine();
            }

            if (odgovor.Equals("D"))
            {
                Console.Clear();
                goto odabirOpcija1;
            }
            else
            {
                Console.WriteLine("\nHvala Vam na saradnji, prijatno!\n");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

                Environment.Exit(0);
            }
        }
    }
}
