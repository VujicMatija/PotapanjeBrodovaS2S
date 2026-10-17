using PotapanjeBrodovaS2S.Models;
using System.Diagnostics;

namespace PotapanjeBrodovaS2S.BrokerBazePodataka
{
    public class SoPrijavaIgraca : SOBase
    {
        private Igrac igrac;
        public Igrac? Result;

        public SoPrijavaIgraca(Igrac igrac)
        {
            this.igrac = igrac;
            
        }

        public override void SOFunction()
        {
            igrac.WhereUslov = $"Nadimak = '{igrac.Nadimak}' and Lozinka = '{igrac.Lozinka}'";
            Debug.WriteLine(igrac.WhereUslov);
            List <Igrac> lista= broker.GetByCondition(igrac).Cast<Igrac>().ToList();
            Result = lista.FirstOrDefault();
            if(Result == null)
            {
                throw new Exception("Ne postoji korisnik sa ovim kredencijalima");
            }
        }


        //Nakon ovoga servis baze implementiramo
    }
}
