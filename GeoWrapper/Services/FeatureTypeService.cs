using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using GeoWrapper.Models;
using Newtonsoft.Json;

namespace GeoWrapper.Services
{
    public class FeatureTypeService : FlurlBaseClass
    {
        public FeatureTypeService(IFlurlClientFactory flurlClientFac, AuthDataContainer authDataContainer) : base(flurlClientFac, authDataContainer)
        {
        }

        public async Task<ICollection<FeatureType>> GetFeatureTypes(string workspaceName, string dataStoreName)
        {
            var featureTypeResponse = await Request("workspaces", workspaceName, "datastores", dataStoreName, "featuretypes.json")
                //.SetQueryParam("list", "available")
                .GetJsonAsync<FeatureTypeResponse>();
            return featureTypeResponse?.FeatureTypeList?.FeatureTypes;
        }

        public async Task<bool> CreateFeatureType(string workspaceName, string dataStoreName, FeatureTypeDetailInfo featureTypeDetailInfo)
        {
            try
            {
                var created = await Request("workspaces", workspaceName, "datastores", dataStoreName, "featuretypes")
                    .PostJsonAsync(new FeatureTypeDetailInfoContainer{ FeatureTypeDetailInfo = featureTypeDetailInfo});
                return created.StatusCode == 201;
            }
            catch (FlurlHttpException)
            {
                return false;
            }
        }

        public async Task<FeatureTypeDetailInfo> GetFeatureType(string workspaceName, string dataStoreName, string featureTypeName)
        {
            var featureTypeDetailInfoContainer = await Request("workspaces", workspaceName, "datastores", dataStoreName, "featuretypes", featureTypeName + ".json")
                .GetJsonAsync< FeatureTypeDetailInfoContainer>();
            return featureTypeDetailInfoContainer?.FeatureTypeDetailInfo;
        }

        public async Task UpdateFeatureType(string workspaceName, string dataStoreName, string featureTypeName, FeatureTypeDetailInfo featureTypeDetailInfo)
        {
            await Request("workspaces", workspaceName, "datastores", dataStoreName, "featuretypes", featureTypeName + ".json")
                .PutJsonAsync(new FeatureTypeDetailInfoContainer { FeatureTypeDetailInfo = featureTypeDetailInfo });
        }

        public async Task DeleteFeatureType(string workspaceName, string dataStoreName, string featureTypeName)
        {
            await Request("workspaces", workspaceName, "datastores", dataStoreName, "featuretypes", featureTypeName + ".json")
                .DeleteAsync();
        }
    }

    public class FeatureTypeDetailInfoContainer
    {
        [JsonProperty("featureType")]
        public FeatureTypeDetailInfo FeatureTypeDetailInfo { get; set; }
    }

    class FeatureTypeResponse
    {
        [JsonProperty("featureTypes")]
        public FeatureTypeList FeatureTypeList { get; set; }
    }

    class FeatureTypeList
    {
        [JsonProperty("featureType")]
        public ICollection<FeatureType> FeatureTypes { get; set; }
    }
}
