using CicekSepeti.Data.Interface;

namespace CicekSepeti.Data.Concrete
{
    public class CicekSepetiDatabaseSettings:ICicekSepetiDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }



    }
}
