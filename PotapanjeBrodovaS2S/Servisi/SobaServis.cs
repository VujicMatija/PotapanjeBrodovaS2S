using PotapanjeBrodovaS2S.Models;
using System.Collections.Concurrent;

namespace PotapanjeBrodovaS2S.Servisi
{
    public class SobaServis
    {

        private readonly ConcurrentDictionary<string, Soba> _sobe = new();


        public Soba KreirajSobu(Igrac igrac)
        {
            var rng = new Random();
            string kod = "";
            do
            {
                kod = rng.Next(1000, 9999).ToString();
            }while(_sobe.ContainsKey(kod));

            var soba = new Soba { Kod = kod };
            soba.IgracA = igrac;
            _sobe[kod] = soba;
            return soba;
        }

        public Soba? PridruziSeSobi(string kod, Igrac igrac)
        {
            if(_sobe.TryGetValue(kod, out var soba) && soba.IgracB == null)
            {
                soba.IgracB = igrac;
                soba.Spremna = true;
                return soba;
            }

            return null;
        }

        public Soba? PronadjiSobu(string kod)
        {
             _sobe.TryGetValue(kod, out var soba);
            return soba;
        }

        public bool PosaljiTablu(string kod, string nadimak, Tabla tabla)
        {
            // vraca da li je soba spremna ili ne nakon slanja table
            Soba? soba = PronadjiSobu(kod);
            if (soba == null) return false;
            // ako do sada nisi objasnio referentne tipovi objasni ih sada
            Igrac igrac = soba.IgracA.Nadimak == nadimak ? soba.IgracA : soba.IgracB;
            igrac.Tabla = tabla;
            if (soba.IgracA.Tabla != null && soba.IgracB.Tabla != null)
            {
                return true;
            }
            else return false;
        }

    }
}
