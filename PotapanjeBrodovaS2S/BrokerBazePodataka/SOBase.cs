namespace PotapanjeBrodovaS2S.BrokerBazePodataka
{
    public abstract class SOBase
    {

        //Abstraktna klasa je klasa koja ne moze da se instancira vec da se nasledjuje
        //mora da sadrzi barem jednu apstraktnu funkciju koja klasa koja je dete abstraktne klase mora da implementura
        //moze da ima implementaciju klasa ili svojstva
        protected Broker broker;

        public SOBase()
        {
            broker = new Broker();
        }

        public void ExecuteSystemOperation()
        {
            try
            {
                broker.OpenConnection();
                broker.BeginTransaction();

                SOFunction();
                
                broker.Commit();
            }
            catch(Exception ex)
            {
                broker.Rollback();
            }
            finally
            {
                broker.CloseConnection();
            }
        }

        public abstract void SOFunction();

    }
}
