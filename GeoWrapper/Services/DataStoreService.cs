using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using GeoWrapper.Models;
using Newtonsoft.Json;

namespace GeoWrapper.Services
{
    public class DataStoreService : FlurlBaseClass
    {
        public DataStoreService(IFlurlClientFactory flurlClientFac, AuthDataContainer authDataContainer) : base(flurlClientFac, authDataContainer)
        {
        }

        public async Task<ICollection<DataStore>> GetDateStores(string workspaceName)
        {
            var dataStoreResponse = await Request("workspaces", workspaceName, "datastores")
                .GetJsonAsync<DataStoreResponse>();
            return dataStoreResponse?.DateStoreList?.DataStores;
        }

        public async Task<bool> CreateDataStore(string workspaceName, DataStoreDetailInfo dataStoreDetailInfo)
        {
            try
            {
                var created = await Request("workspaces", workspaceName, "datastores")
                    .PostJsonAsync(new DateStoreContainer{ DataStoreDetailInfo = dataStoreDetailInfo});
                return created.StatusCode == 201;
            }
            catch (FlurlHttpException)
            {
                return false;
            }
        }

        public async Task<DataStoreDetailInfo> GetDataStore(string workspaceName, string dataStoreName)
        {
            var detailWorkSpaceResponse = await Request("workspaces", workspaceName, "datastores", dataStoreName)
                .GetJsonAsync<DateStoreContainer>();
            return detailWorkSpaceResponse.DataStoreDetailInfo;
        }

        public async Task UpdateWorkspace(string workspaceName, string dataStoreName, DataStoreDetailInfo dataStoreDetailInfo)
        {
            await Request("workspaces", workspaceName, "datastores", dataStoreName)
                .PutJsonAsync(new DateStoreContainer { DataStoreDetailInfo = dataStoreDetailInfo });
        }

        public async Task DeleteDataStore(string workspaceName, string dataStoreName)
        {
            await Request("workspaces", workspaceName, "datastores", dataStoreName)
                .DeleteAsync();
        }
    }

    class DataStoreResponse
    {
        [JsonProperty("dataStores")]
        public DateStoreList DateStoreList { get; set; }
    }

    class DateStoreList
    {
        [JsonProperty("dataStore")]
        public ICollection<DataStore> DataStores { get; set; }
    }

    class DateStoreContainer
    {
        [JsonProperty("dataStore")]
        public DataStoreDetailInfo DataStoreDetailInfo { get; set; }
    }
}
