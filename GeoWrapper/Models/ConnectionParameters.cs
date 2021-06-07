using System.Collections.Generic;
using GeoWrapper.Services;
using Newtonsoft.Json;

namespace GeoWrapper.Models
{
    public class ConnectionParameters
    {
        [JsonProperty("entry")]
        public ICollection<ConnectionParameterEntry> Entries { get; set; }

        public static ConnectionParameters Create(DataStoreSqlParams dataStoreSqlParams)
        {
            return new ConnectionParameters
            {
                Entries = new List<ConnectionParameterEntry>
                {
                    ConnectionParameterEntry.Create("host", dataStoreSqlParams.Server),
                    ConnectionParameterEntry.Create("port", dataStoreSqlParams.Port.ToString()),
                    ConnectionParameterEntry.Create("schema", dataStoreSqlParams.Schema),
                    ConnectionParameterEntry.Create("database", dataStoreSqlParams.Database),
                    ConnectionParameterEntry.Create("user", dataStoreSqlParams.User),
                    ConnectionParameterEntry.Create("passwd", dataStoreSqlParams.Password),
                    ConnectionParameterEntry.Create("dbtype", dataStoreSqlParams.DbType),
                    ConnectionParameterEntry.Create("create database", dataStoreSqlParams.CreateDb.ToString()),
                    ConnectionParameterEntry.Create("Loose bbox", dataStoreSqlParams.LooseBbox.ToString()),
                    ConnectionParameterEntry.Create("Estimated extends", dataStoreSqlParams.EstimatedExtends.ToString()),
                }
            };
        }
    }
}
