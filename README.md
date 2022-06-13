Banka -
Potrebno je kreirati konzolnu aplikaciju koja simulira rad banaka.
Klijent može da zahteva kredit od banke. Korisnik unosi naziv banke, lične podatke
klijenta (ime, jmbg, mesečna primanja, godine radnog staza), iznos kredita, broj
mesečnih rata. Banka može da odobri ili ne odobri kredit. Svaka banka ima svoja
pravila na osnovu kojih odobrava ili ne odobrava kredit.
Kreirati tri banke RBC, Santander, Wells Fargo . RBC odbija kredit ako klijent ima
neki aktivan (neisplaćen) kredit u RBC-ju, ili ukoliko plata nije bar duplo veća od
rate. Santander odobrava kredit ukoliko klijent ima preko 5 godina radnog staža i
ukoliko je iznos mesečne rate manji od 70% mesečnog pirmanja klijenta. Wells
Fargo odobrava kredit ukoliko su mesečna primanja klijenta veća od rate kredita.
Učitati listu kreditnih aplikacija iz csv fajla (kolone: naziv banke, ime klijenta, jmbg,
mesečna primanja, godine radnog staža, iznos kredita, broj mesečnih rata), za svakog
klijenta aplicirati za kredit.
Sa stadnardnog ulaza korisnik bira jednu od tri opcije:

    • Korisnik može da plati ratu ili da dobije informacije o kreditima. Da bi
        platio ratu, korisnik unosi (sa standardnog ulaza) jmbg klijenta i naziv
        banke. Ukoliko klijent ima više aktivnih kredita, prikazuju se aktivni
        krediti, i korisnik treba da izabere za koji kredit plaća ratu.

    • Korisnik može da izabere opciju za prikazivanje informacija o
        kreditima, unosi jmbg klijenta i naziv banke (sa standardnog ulaza).
        Ispisati informacije o kreditima na standardnom izlazu.
        
    • Korisnik može naknadno da unese nove aplikacije. On unosi putanju
        do csv fajla odakle se učitavaju kreditne aplikacije.
