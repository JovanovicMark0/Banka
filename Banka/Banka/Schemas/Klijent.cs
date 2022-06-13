using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Banka.Schemas
{
    public class Klijent
    {
        public string ime { get; set; }
        public string JMBG { get; set; }
        public int mesecnaPrimanja { get; set; }
        public int godineRadnogStaza { get; set; }

        public Klijent(string ime, string JMBG, int mesecnaPrimanja, int godineRadnogStaza)
        {
            this.ime = ime;
            this.JMBG = JMBG;
            this.mesecnaPrimanja = mesecnaPrimanja;
            this.godineRadnogStaza = godineRadnogStaza;
        }

        public bool apliciranjeZaKredit(string nazivBanke, int iznosKredita, int brojMesecnihRata)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška prilikom apliciranja! \n {0}", ex.Message);
                return false;
            }
            return true;
        }
    }
}