using Microsoft.Data.SqlClient;

namespace PotapanjeBrodovaS2S.Models
{
    public interface OpstiDomenskiObjekat
    {

        //Interfejs predstavlja spisak metoda ili svojstva bez implementacije koje klasa koja nasledjuje mora da implementira
        string NazivTabele { get; }
        string Vrednosti { get; }

        string WhereUslov { get; set; }
        object UpdateVrednosti { get; }

        List<OpstiDomenskiObjekat> GetReaderList(SqlDataReader reader);
    }
}
