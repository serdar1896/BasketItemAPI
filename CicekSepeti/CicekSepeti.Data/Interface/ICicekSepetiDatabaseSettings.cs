namespace CicekSepeti.Data.Interface
{
    public interface ICicekSepetiDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
