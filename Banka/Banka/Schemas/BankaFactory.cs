using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka.Schemas
{
    public class BankaFactory
    {
        public Banka makeBanka(string nazivBanke)
        {
            if (nazivBanke.ToLower().Equals("rbc"))
                return new RBC();
            else if (nazivBanke.ToLower().Equals("santander"))
                return new Santander();
            else if (nazivBanke.ToLower().Equals("wells fargo"))
                return new Wells_Fargo();
            else return null;
        }
    }
}
