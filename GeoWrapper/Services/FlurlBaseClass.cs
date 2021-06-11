using System.Diagnostics.CodeAnalysis;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace GeoWrapper.Services
{
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public class FlurlBaseClass
    {
        private IFlurlClient _flurlClient;
        private readonly IFlurlClientFactory _flurlClientFac;
        private readonly AuthDataContainer _authDataContainer;

        public FlurlBaseClass(IFlurlClientFactory flurlClientFac, AuthDataContainer authDataContainer)
        {
            _flurlClientFac = flurlClientFac;
            _authDataContainer = authDataContainer;
        }

        protected IFlurlRequest Request(params string[] segments)
        {
            if (_flurlClient == null || _flurlClient.BaseUrl != _authDataContainer.Server)
            {
                _flurlClient?.Dispose();
                _flurlClient = _flurlClientFac.Get(_authDataContainer.Server);
                _flurlClient.WithBasicAuth(_authDataContainer.Login, _authDataContainer.Password);
            }
            return segments?.Length > 0 ? _flurlClient.Request().AppendPathSegments(segments) : _flurlClient.Request();
        }
    }
}
