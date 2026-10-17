using Microsoft.Data.SqlClient;

namespace PotapanjeBrodovaS2S.BrokerBazePodataka
{
    public class DbConnection
    {

        // prvo u SSMS-u napraviti bazu
        // nakon kreiranja baze u SSMS-u objasniti transakcije i kako dovesti bazu iz jednog dopustivog stanja do drugog
        private SqlTransaction transaction;
        private SqlConnection connection;

        public DbConnection()
        {
            connection = new SqlConnection(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PotapanjeBrodova;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True");


        }

        public void OpenConnection()
        {
            connection?.Open();
        }

        public void CloseConnection()
        {
            connection?.Close();
        }

        public void BeginTransaction()
        {
            transaction = connection.BeginTransaction();
        }
        public void Commit()
        {
            transaction?.Commit();
        }
        public void Rollback()
        {
            transaction?.Rollback();
        }
        
        public SqlCommand CreateCommand()
        {
            return new SqlCommand("", connection, transaction);
        }
    }
}
