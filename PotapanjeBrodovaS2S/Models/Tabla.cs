using Microsoft.AspNetCore.Routing.Matching;
using PotapanjeBrodovaS2S.Models.Utils;

namespace PotapanjeBrodovaS2S.Models
{
    public class Tabla
    {
        // ne moze matrica jer ne podrzava json
        public Polje[][] Polja { get; set; }

        public int maxJednoPolje { get; set; } = 4;
        public int maxDvaPolja { get; set; } = 3;
        public int maxTriPolja { get; set; } = 2;
        public int maxCetriPolja { get; set; } = 1;

        public int trenutnoJednoPolje { get; set; } = 0;
        public int trenutnoDvaPolja { get; set; } = 0;
        public int trenutnoTriPolja { get; set; } = 0;
        public int trenutnoCetiriPolja { get; set; } = 0;

        public int ukupnoPostavljeno { get; set; } = 0;

        public Tabla()
        {
            Polja = InicijalizujTablu();
        }
        // dovde na pocetku ostalo blagovremeno, sada predji na tabla komponentu

        //public Polje this[int red, Slovo kolona]
        //{
        //    get => Polja[red][(int)kolona];
        //    set => Polja[red][(int)kolona] = value;
        //}

        public Polje[][] InicijalizujTablu()
        {
            Polja = new Polje[10][];
            for (int i = 0; i < 10; i++)
            {
                Polja[i] = new Polje[10];
                for (int j = 0; j < 10; j++)
                {
                    Polja[i][j] = new Polje
                    {
                        Red = i,
                        Kolona = (Slovo)j
                    };
                }
            }

            return Polja;
        }


        public bool ProveriPreostalebrodove(int duzina)
        {
            if (duzina == 1)
            {
                return trenutnoJednoPolje < maxJednoPolje;
            }
            else if (duzina == 2)
            {
                return trenutnoDvaPolja < maxDvaPolja;
            }
            else if (duzina == 3)
            {
                return trenutnoTriPolja < maxTriPolja;
            }
            else if(duzina == 4)
            {
                return trenutnoCetiriPolja < maxCetriPolja;
            }else return false;
        }


        public void SmanjiPreostaleBrodove(int duzinaBroda)
        {
            if (duzinaBroda == 1)
            {
                trenutnoJednoPolje++;
            }
            else if (duzinaBroda == 2)
            {
                trenutnoDvaPolja++;
            }
            else if (duzinaBroda == 3)
            {
                trenutnoTriPolja++;
            }
            else if(duzinaBroda == 4)
            {
                trenutnoCetiriPolja++;
            }
        }

        public  void PostaviBrod(Polje polje, int duzinaBroda, bool horizontalno )
        {
            try
            {
                //prvo proveravamo da li imamo dovoljno brodova na raspolaganju i pravimo fju za to
                if (!ProveriPreostalebrodove(duzinaBroda))
                {
                    //await JS.InvokeVoidAsync("alert", "Nemate odabrani tip brodova na raspolaganju");
                    throw new Exception("Nemate odabrani tip brodova na raspolaganju");
                    
                }
                // prvo moramo da vidimo da li mozemo uopste da postavimo brod
                bool mozeDaSePostavi = true;

                if (horizontalno)
                {
                    //pravimo parametre for petlje
                    int i = (int)polje.Kolona;
                    int duzina = i + (int)duzinaBroda;
                    try
                    {

                        for (i = (int)polje.Kolona; i < duzina; i++)
                        {
                            if (this.Polja[polje.Red][i].Zauzeto || this.Polja[polje.Red][i].Nedostupno)
                            {
                                mozeDaSePostavi = false;

                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        //await JS.InvokeVoidAsync("alert", "los unos");
                        throw new Exception("Los unos, brod ne moze da stane");
                        
                    }

                    if (mozeDaSePostavi)
                    {

                        i = (int)polje.Kolona;
                        for (i = (int)polje.Kolona; i < duzina; i++)
                        {
                            this.Polja[polje.Red][i].Zauzeto = true;
                        }

                        // dodajemo koliko smo brodova posavili
                        ukupnoPostavljeno += (int)duzinaBroda;
                        // smanjujemo dostupnost brodova
                        SmanjiPreostaleBrodove(duzinaBroda);
                        // obelezavamo polja nedostupnim
                        ObeleziPoljaNedostupnim();
                        

                    }
                    else
                    {
                        //await JS.InvokeVoidAsync("alert", "Ne mozete da postavite brod na zauzeta polja");
                        throw new Exception("Ne mozete da postavite brod na zauzeta polja");
                    }

                }
                else
                {

                    int i = polje.Red;
                    int duzina = i + (int)duzinaBroda;
                    try
                    {
                        for (i = polje.Red; i < duzina; i++)
                        {
                            if (this.Polja[i][(int)polje.Kolona].Zauzeto || this.Polja[i][(int)polje.Kolona].Nedostupno)
                            {
                                mozeDaSePostavi = false;
                            }
                        }
                    }
                    catch
                    {
                        //await JS.InvokeVoidAsync("alert", "los unos");
                        throw new Exception("Los unos, brod ne moze da stane");
                        
                    }
                    if (mozeDaSePostavi)
                    {
                        i = polje.Red;
                        for (i = polje.Red; i < duzina; i++)
                        {
                            this.Polja[i][(int)polje.Kolona].Zauzeto = true;


                        }



                        ukupnoPostavljeno += (int)duzinaBroda;
                        ObeleziPoljaNedostupnim();
                        SmanjiPreostaleBrodove(duzinaBroda);
                        

                    }
                    else
                    {
                        //await JS.InvokeVoidAsync("alert", "Ne mozete da postavite brod na zauzeta polja");
                        throw new Exception("Ne mozete da postavite brod na zauzeta polja");
                    }

                }
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void ObeleziPoljaNedostupnim()
        {
            foreach (Polje[] red in this.Polja)
            {
                foreach (Polje p in red)
                {
                    if (p.Zauzeto == true)
                    {
                        foreach (Polje sused in p.NadjiSusede())
                        {
                            if (this.Polja[sused.Red][(int)sused.Kolona].Zauzeto == false)
                            {
                                this.Polja[sused.Red][(int)sused.Kolona].Nedostupno = true;
                            }
                        }
                    }
                }
            }
        }

       
        public List<Polje> PronadjiBrod(Polje polje, List<Polje> lista = null)
        {
            lista = lista ?? new List<Polje>();

            lista.Add(polje);
            foreach(Polje p in polje.NadjiSusede())
            {
                Polje sused = this.Polja[p.Red][(int)p.Kolona];
                if(sused.Zauzeto && lista.Contains(sused) == false)
                {
                    PronadjiBrod(sused, lista);
                }
            }

            return lista;
        }

        public void IzbirisiBrod(Polje polje, int duzinaBroda)
        {
            
            if(polje != null && polje.Zauzeto)
            {
                
                foreach (Polje p in PronadjiBrod(polje))
                {
                    this.Polja[p.Red][(int)p.Kolona].Zauzeto = false;
                }

                foreach (Polje p in PronadjiBrod(polje))
                {
                    
                    p.Zauzeto = false; // nmisam siguran sto sam stavio ovo ovde
                    foreach (Polje sused in p.NadjiSusede())
                    {
                        this.Polja[sused.Red][(int)sused.Kolona].Nedostupno = false;
                    }
                }
                //prvo pokazi kako radi da se vidi da nepravilno brise pa dodaj ovo al pre toga uradi ovbo ispod
                ObeleziPoljaNedostupnim();
            }
            else
            {
                return;
            }

            if (duzinaBroda == 1)
            {
                trenutnoJednoPolje--;
                ukupnoPostavljeno -= 1;
            }
            else if (duzinaBroda == 2)
            {
                trenutnoDvaPolja--;
                ukupnoPostavljeno -= 2;
            }
            else if (duzinaBroda == 3)
            {
                trenutnoTriPolja--;
                ukupnoPostavljeno -= 3;
            }
            else
            {
                trenutnoCetiriPolja--;
                ukupnoPostavljeno -= 4;
            }

        }
    }
}
