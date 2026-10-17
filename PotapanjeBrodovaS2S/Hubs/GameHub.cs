using Microsoft.AspNetCore.SignalR;
using PotapanjeBrodovaS2S.Components.Pages;
using PotapanjeBrodovaS2S.Models;
using PotapanjeBrodovaS2S.Models.Utils;
using PotapanjeBrodovaS2S.Servisi;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;

namespace PotapanjeBrodovaS2S.Hubs
{
    public class GameHub : Hub
    {
        private readonly BazaServis _bazaServis;
        private readonly SobaServis _sobaServis;

        public GameHub(BazaServis bazaServis, SobaServis sobaServis)
        {
            _bazaServis = bazaServis;
            _sobaServis = sobaServis;
        }
        public Igrac? RegistrujIgraca(Igrac igrac)
        {
            return _bazaServis.DodajIgraca(igrac);
            
        }

        public Igrac? PrijaviIgraca(Igrac igrac)
        {
            return _bazaServis.PrijaviIgraca(igrac);
        }

        public Soba KreirajSobu(Igrac igrac)
        {
            // prvo implementirati 
            return _sobaServis.KreirajSobu(igrac);
        }

        public async Task<Soba?> PridruziSeSobi(string kod, Igrac igrac)
        {
            var soba =  _sobaServis.PridruziSeSobi(kod, igrac);
            if(soba == null)
            {
                return null;
            }
            soba.Spremna = true;
            await Clients.Client(soba.IgracA.ConnectionID).SendAsync("SobaSpremna", soba);
            return soba;
        }

        public async Task PokreniIgru(string kod)
        {
            await Clients.Client(_sobaServis.PronadjiSobu(kod).IgracB.ConnectionID).SendAsync("PokreniIgru");
        }

        public async Task PosaljiTablu(Tabla tabla, string nadimak, string kod)
        {
            //dodajemo tablu i vidmo da li je soba spremna 
            bool spremniZaBitku =  _sobaServis.PosaljiTablu(kod, nadimak, tabla);
            if (spremniZaBitku)
            {
                var soba = _sobaServis.PronadjiSobu(kod);
                // da bi prvi na potezu vbio random pravimo novcic
                int novicic = Random.Shared.Next(0, 2);
                await Clients.Clients(soba.IgracA.ConnectionID).SendAsync("PokreniBitku", novicic);
                await Clients.Clients(soba.IgracB.ConnectionID).SendAsync("PokreniBitku", 1 - novicic);
            }
        }

        public async Task PosaljiPotez(Polje polje, string kod, string nadimak)
        {
            var soba = _sobaServis.PronadjiSobu(kod);
            RezultatPoteza rez = RezultatPoteza.PROMASENO;
            Tabla protivnikovaTabla = soba.IgracA.Nadimak == nadimak ? soba.IgracB.Tabla : soba.IgracA.Tabla;
            List<Polje> poljaObuhvacenaPotezom = new List<Polje>();
            Polje pogodjenoPolje = protivnikovaTabla.Polja[polje.Red][(int)polje.Kolona];
            if (pogodjenoPolje.Zauzeto == false)
            {
                poljaObuhvacenaPotezom.Add(pogodjenoPolje);
            }else
            {
                pogodjenoPolje.Pogodjeno = true;
                var brod = protivnikovaTabla.PronadjiBrod(pogodjenoPolje);
                if(brod.All(polje => polje.Pogodjeno== true))
                {
                    rez = RezultatPoteza.POTPOLJEN;
                    poljaObuhvacenaPotezom = brod;
                }
                else
                {
                    rez = RezultatPoteza.RANJEN;
                    poljaObuhvacenaPotezom.Add(polje);
                }
            }

            Clients.Clients(soba.IgracA.ConnectionID, soba.IgracB.ConnectionID).SendAsync("OdigranPotez", poljaObuhvacenaPotezom, rez, nadimak);
        }

        public async void ProglasiPobedu(string nadimak, string kod)
        {
            var soba = _sobaServis.PronadjiSobu(kod);
            Clients.Clients(soba.IgracA.ConnectionID, soba.IgracB.ConnectionID).SendAsync("OglasiPobedu", $"{nadimak} je pobedio!");
        }
    }
}
    