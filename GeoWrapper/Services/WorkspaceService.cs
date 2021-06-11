using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GeoWrapper.Models;
using Flurl.Http;
using Newtonsoft.Json;
using Flurl.Http.Configuration;

namespace GeoWrapper.Services
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    public class WorkspaceService : FlurlBaseClass
    {
        public WorkspaceService(IFlurlClientFactory flurlClientFac, AuthDataContainer authDataContainer) : base(flurlClientFac, authDataContainer)
        {
        }

        public async Task<ICollection<Workspace>> GetWorkspaces()
        {
            var workspaceResponse = await Request("workspaces")
                .GetJsonAsync<WorkSpacesResponse>();
            return workspaceResponse?.WorkSpaceList?.Workspaces;
        }

        public async Task<bool> CreateWorkspace(string name, string href)
        {
            try
            {
                var created = await Request("workspaces")
                    .PostJsonAsync(new
                    {
                        workspace = new
                        {
                            name,
                            href
                        }
                    });
                return created.StatusCode == 201;
            }
            catch (FlurlHttpException)
            {
                return false;
            }
        }

        public async Task<WorkspaceDetailInfo> GetWorkspace(string name)
        {
            var detailWorkSpaceResponse = await Request("workspaces", name)
                .GetJsonAsync<WorkSpaceContainer>();
            return detailWorkSpaceResponse.WorkspaceDetailInfo;
        }

        public async Task UpdateWorkspace(string name, WorkspaceDetailInfo workspaceDetailInfo)
        {
            await Request("workspaces", name)
                .PutJsonAsync(new WorkSpaceContainer { WorkspaceDetailInfo = workspaceDetailInfo });
        }

        public async Task DeleteWorkspace(string name)
        {
            await Request("workspaces", name)
                .DeleteAsync();
        }
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    class WorkSpacesResponse
    {
        [JsonProperty("workspaces")]
        public WorkSpaceList WorkSpaceList { get; set; }
    }

    class WorkSpaceList
    {
        [JsonProperty("workspace")]
        public ICollection<Workspace> Workspaces { get; set; }
    }

    class WorkSpaceContainer
    {
        [JsonProperty("workspace")]
        public WorkspaceDetailInfo WorkspaceDetailInfo { get; set; }
    }
}