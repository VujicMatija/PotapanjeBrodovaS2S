using Microsoft.Data.SqlClient;
using PotapanjeBrodovaS2S.Models;
using System.Diagnostics;

namespace PotapanjeBrodovaS2S.BrokerBazePodataka
{
    public class Broker
    {

        private DbConnection connection;

        public Broker()
        {
            connection = new DbConnection();
        }

        public void OpenConnection()
        {
            connection.OpenConnection();
        }

        public void CloseConnection()
        {
            connection.CloseConnection();
        }

        public void BeginTransaction()
        {
            connection.BeginTransaction();
        }

        public void Commit()
        {
            connection.Commit();
        }

        public void Rollback()
        {
            connection.Rollback();
        }

        public void Add(OpstiDomenskiObjekat odo)
        {
            SqlCommand command = connection.CreateCommand();
            //Ovde sad krenuti da se objasnjava Opsti domenski objekat
            command.CommandText = $"insert into {odo.NazivTabele} values ({odo.Vrednosti})";
            Console.WriteLine(">>>>>>>>> " + command.CommandText);
            command.ExecuteNonQuery();
            command.Dispose();
            
        }

        public List<OpstiDomenskiObjekat> GetAll(OpstiDomenskiObjekat odo)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"Select * from {odo.NazivTabele}";
            using SqlDataReader reader = command.ExecuteReader();
            List<OpstiDomenskiObjekat> list = odo.GetReaderList(reader);
            command.Dispose();
            return list;
        }

        public List<OpstiDomenskiObjekat> GetByCondition(OpstiDomenskiObjekat odo)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"Select * from {odo.NazivTabele} where {odo.WhereUslov}";
            Debug.WriteLine(">>>>" + command.CommandText);
            using SqlDataReader reader = command.ExecuteReader();
            List<OpstiDomenskiObjekat> list = odo.GetReaderList(reader);
            command.Dispose();
            return list;
        }

        // ove ne koristimo al aj

        public void Delete(OpstiDomenskiObjekat odo)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"Delete from {odo.NazivTabele} where {odo.WhereUslov}";
            command.ExecuteNonQuery();
            command.Dispose();
        }

        public void UpdateByCondition(OpstiDomenskiObjekat odo)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = $"Update {odo.NazivTabele} {odo.UpdateVrednosti} where {odo.WhereUslov}";
            command.ExecuteNonQuery();
            command.Dispose();
        }

        //Sada prelazimo na SO
    }
}
