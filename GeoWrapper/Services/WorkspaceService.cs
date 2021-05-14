using System.Collections.Generic;
using System.Threading.Tasks;
using GeoWrapper.Models;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Flurl.Http.Configuration;

namespace GeoWrapper.Services
{
    public class WorkspaceService
    {
        private IFlurlClient _flurlClient;
        private readonly IFlurlClientFactory _flurlClientFac;
        private string _server;
        private string _login;
        private string _password;

        public WorkspaceService(IFlurlClientFactory flurlClientFac)
        {
            _flurlClientFac = flurlClientFac;
        }

        public void Configure(string server, string login, string password)
        {
            _server = server;
            _login = login;
            _password = password;
        }

        private IFlurlRequest request()
        {
            if (_flurlClient == null)
            {
                _flurlClient = _flurlClientFac.Get(_server);
                _flurlClient.WithBasicAuth(_login, _password);
            }

            return _flurlClient.Request().AppendPathSegment("workspaces");
        }

        public async Task<ICollection<Workspace>> GetWorkspaces()
        {
            var workspaceResponse = await request().GetJsonAsync<WorkspaceResponse>();
            return workspaceResponse?.Workspaces?.Workspace;
        }

        public async Task<bool> AddWorkspace(string Name, string Href)
        {
            var created = await request().PostJsonAsync(new
            {
                name = Name,
                href = Href
            }).ReceiveString();
            return created == Name;
        }
    }

    class WorkspaceResponse
    {
        [JsonProperty("workspaces")]
        public WorkspaceList Workspaces { get; set; }
    }

    class WorkspaceList
    {
        [JsonProperty("workspace")]
        public ICollection<Workspace> Workspace { get; set; }
    }
}