using PotapanjeBrodovaS2S.BrokerBazePodataka;
using PotapanjeBrodovaS2S.Models;

namespace PotapanjeBrodovaS2S.Servisi
{
    public class BazaServis
    {
        public Igrac DodajIgraca(Igrac igrac)
        {
            SoDodajIgraca so = new SoDodajIgraca(igrac);
            so.ExecuteSystemOperation();
            return so.Result;
        }

        internal Igrac? PrijaviIgraca(Igrac igrac)
        {
            SoPrijavaIgraca so = new SoPrijavaIgraca(igrac);
            so.ExecuteSystemOperation();
            return so.Result;
        }


        //Sada ga u program.cs dodati
    }
}
