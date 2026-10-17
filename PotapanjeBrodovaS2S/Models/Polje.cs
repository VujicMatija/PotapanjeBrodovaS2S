using PotapanjeBrodovaS2S.Models.Utils;

namespace PotapanjeBrodovaS2S.Models
{
    public class Polje
    {
        public Slovo Kolona {  get; set; }
        public int Red { get; set; }

        public RezultatPoteza Stanje { get; set; } = RezultatPoteza.NEODIGRANO;

        public bool Zauzeto { get; set; } = false;
        public bool Pogodjeno { get; set; } = false;
        public bool Nedostupno { get; set; } = false;





        public List<Polje> NadjiSusede()
        {
            List<Polje> res = new List<Polje>();
            List<Slovo> susedneKolone = new List<Slovo>();
            susedneKolone.Add(Kolona);
            if (Kolona == Slovo.A)
            {
                susedneKolone.Add(Slovo.B);
            }
            else if (Kolona == Slovo.I)
            {
                susedneKolone.Add(Slovo.J);
            }
            else
            {
                susedneKolone.Add(Kolona - 1);
                susedneKolone.Add(Kolona + 1);
            }

            List<int> susedniBrojevi = new List<int>();
            susedniBrojevi.Add(Red);
            if (Red == 0)
            {
                susedniBrojevi.Add(1);
            }
            else if (Red == 9)
            {
                susedniBrojevi.Add(8);
            }
            else
            {
                susedniBrojevi.Add(Red - 1);
                susedniBrojevi.Add(Red + 1);
            }

            foreach (Slovo s in susedneKolone)
            {
                foreach (int i in susedniBrojevi)
                {
                    Polje p = new Polje
                    {
                        Red = i,
                        Kolona = s
                    };

                    if (!p.Equals(this))
                    {
                        res.Add(p);
                    }

                }
            }

            return res;
        }

        override public string ToString()
        {
            return $"{Kolona}{Red + 1}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Polje polje && Red == polje.Red && Kolona == polje.Kolona;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Red, Kolona);
        }
    }
}
