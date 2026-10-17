namespace PotapanjeBrodovaS2S.Models
{
    public class Soba
    {
        public string Kod { get; set; }
        public Igrac IgracA { get; set; }
        public Igrac IgracB { get; set; }

        public bool Spremna { get; set; } = false;
    }
}
