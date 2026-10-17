using Microsoft.Data.SqlClient;
using System.Net;

namespace PotapanjeBrodovaS2S.Models
{
    public class Igrac : OpstiDomenskiObjekat
    {
        public string Nadimak { get; set; }
        public string Lozinka { get; set; }
        
        public string ConnectionID {  get; set; }

        public Tabla? Tabla { get; set; } = null; //na pocetku je null da bi mogli da u sobi proveravamo ako obe nisu null da je mozemo da krenemo bitku
        public string NazivTabele => "Igrac";

        public string Vrednosti => $"'{Nadimak}', '{Lozinka}'";

        public string WhereUslov { get; set ; }

        public object UpdateVrednosti { get; set; }

        public List<OpstiDomenskiObjekat> GetReaderList(SqlDataReader reader)
        {
            List<OpstiDomenskiObjekat> res  = new List<OpstiDomenskiObjekat>();
            while (reader.Read())
            {
                Igrac i = new Igrac
                {
                    Nadimak = (string)reader["Nadimak"],
                    Lozinka = (string)reader["Lozinka"]
                };

                res.Add(i);
            }

            return res;
        }

        public override string ToString()
        {
            return $"IGRAC: {Nadimak} {Lozinka}";
        }
    }
}
