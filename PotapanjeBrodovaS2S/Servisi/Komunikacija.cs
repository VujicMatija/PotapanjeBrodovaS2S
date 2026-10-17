using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using PotapanjeBrodovaS2S.Models;
using PotapanjeBrodovaS2S.Models.Utils;

namespace PotapanjeBrodovaS2S.Servisi
{
    public class Komunikacija : IAsyncDisposable
    {

        private readonly HubConnection hubConnection;
        

        public Igrac? prijavljenIgrac = null;

        public Soba? trenutnaSoba = null;

        public bool NaPotezu;
        int bodovi = 0;


        //event - pretplacujemo se na event i kada je pokrenut onda 
        //action je bukv metoda sacuvana u promenljivoj koju mozemo da koristimo 
        public event Action? SobaSpremnaEvent;

        public event Action? PokreniIgruEvent;

        public event Action? PokreniBitkuEvent;

        public event Action<List<Polje>, RezultatPoteza, string>? OdigranPotezEvent;
        public event Action<string>? OglasiPobeduEvent;

        public Komunikacija(NavigationManager nav)
        {
            
            hubConnection = new HubConnectionBuilder().WithUrl(nav.ToAbsoluteUri("/gameHub")).WithAutomaticReconnect().Build();
            //startuj dole
            //povezi ga u program.cs

            hubConnection.On<Soba>("SobaSpremna", (soba) => 
            {
                trenutnaSoba = soba;
                //pokrecemo event na klihjentskoj strani
                SobaSpremnaEvent?.Invoke();
            });


            hubConnection.On("PokreniIgru", () => 
            { 
                PokreniIgruEvent?.Invoke();
            });

            hubConnection.On<int>("PokreniBitku", (novcic) => 
            {
                NaPotezu = novcic == 1 ? true: false;
                PokreniBitkuEvent?.Invoke();
            });

            hubConnection.On<List<Polje>, RezultatPoteza, string>("OdigranPotez", async(polja, rezultat, nadimak) =>
            {

                OdigranPotezEvent.Invoke(polja, rezultat, nadimak);

            });

            hubConnection.On<string>("OglasiPobedu", (poruka) =>
            {
                OglasiPobeduEvent.Invoke(poruka);
            });

            hubConnection.StartAsync();
        }


        //objasniti SingalR i kako funkcionise,
        //Web soketi - Full Duplex konekcija
        //Server Sent Events
        //Long Polling
        //Remote procedure call
        public async Task<Igrac?>  RegistrujIgraca(Igrac i) 
        { 
            return await hubConnection.InvokeAsync<Igrac?>("RegistrujIgraca", i);
            // vracamo se na formu
        }

        public async Task<Igrac?> PrijaviIgraca(Igrac i)
        {
            prijavljenIgrac = await hubConnection.InvokeAsync<Igrac?>("PrijaviIgraca", i);
            if (prijavljenIgrac != null)
            {
                prijavljenIgrac.ConnectionID = hubConnection!.ConnectionId;
            }
            return prijavljenIgrac;
        }

        public async Task<Soba> KreirajSobu()
        {
            return await hubConnection.InvokeAsync<Soba>("KreirajSobu", prijavljenIgrac);
        }

        public async Task<Soba> PridruziSeSobi(string kod)
        {
            trenutnaSoba =  await hubConnection.InvokeAsync<Soba?>("PridruziSeSobi", kod, prijavljenIgrac);
            return trenutnaSoba;
        }

        public async Task PokreniIgru()
        {
            //saljemo kod sobe kako bi server znao kojoj sobi treba da razglasi pocetak igre
            await hubConnection.InvokeAsync("PokreniIgru", trenutnaSoba!.Kod);
        }

        internal  async Task PosaljiTablu(Tabla tabla)
        {
            // saljemo tablu, nadimak kako bi znali koji igrac je poslao i kod da bi znakli sobu
            await hubConnection.InvokeAsync("PosaljiTablu", tabla, prijavljenIgrac!.Nadimak, trenutnaSoba!.Kod);
        }

        public async ValueTask DisposeAsync()
        {
            await hubConnection.DisposeAsync();
        }

        internal async Task PosaljiPotez(Polje polje)
        {
            await hubConnection.InvokeAsync("PosaljiPotez", polje, trenutnaSoba.Kod, prijavljenIgrac.Nadimak);
        }

        internal async Task ProglasiPobedu()
        {
            await hubConnection.InvokeAsync("ProglasiPobedu", prijavljenIgrac.Nadimak, trenutnaSoba.Kod);
        }
    }
}
