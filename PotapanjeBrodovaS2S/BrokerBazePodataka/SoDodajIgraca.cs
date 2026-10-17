using PotapanjeBrodovaS2S.Models;
using System.ComponentModel;


namespace PotapanjeBrodovaS2S.BrokerBazePodataka
{
    public class SoDodajIgraca : SOBase
    {
        private readonly Igrac igrac;
        public Igrac? Result;
        public SoDodajIgraca(Igrac igrac)
        {
            this.igrac = igrac;
        }
        public override void SOFunction()
        {
            //prvo zelimo da vidimo da li postoji korisnik sa ovim usernameom
            igrac.WhereUslov = $"Nadimak = '{igrac.Nadimak}'";
            List<Igrac> lista = broker.GetByCondition(igrac).Cast<Igrac>().ToList(); ;
            if(lista.FirstOrDefault() == null)
            {
                //ako ne postoji onda mozemo da dodamo i vracamo dodatog igraca kao rezultat
                broker.Add(igrac);
                Result = igrac;
            }
            else
            {
                //u supratnom vracamo null
                Result = null;
            }
        }
    }
}
