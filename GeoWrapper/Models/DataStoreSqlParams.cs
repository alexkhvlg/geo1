namespace GeoWrapper.Models
{
    public class DataStoreSqlParams
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DbType { get; set; }
        public string Schema { get; set; }
        public bool CreateDb { get; set; }
        // ReSharper disable once IdentifierTypo
        public bool LooseBbox { get; set; }
        public bool EstimatedExtends { get; set; }

        public DataStoreSqlParams()
        {
            DbType = "postgis";
        }
    }
}
