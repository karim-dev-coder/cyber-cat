using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Client.V3.Infrastructure.WebClientAdapters;
using Shared.Models.Infrastructure.Authorization;

namespace ApiGateway.Client.V3.Infrastructure
{
    internal class WebClient : IWebClient
    {
        public ServerEnvironment ServerEnvironment { get; }

        private readonly IInternalWebClientAdapter _internalWebClient;
        private readonly AuthorizationToken _token;

        public WebClient(ServerEnvironment serverEnvironment, AuthorizationToken token = null)
        {
            _internalWebClient =
#if UNITY_WEBGL
                new ApiGateway.Client.V3.Infrastructure.WebClientAdapters.UnityWebRequest.UnityWebClient();
#endif
#if WEB_CLIENT
                new ApiGateway.Client.V3.Infrastructure.WebClientAdapters.WebClientAdapter.InternalWebClientAdapter();
#endif

            _token = token;
            ServerEnvironment = serverEnvironment;

            if (token != null)
                _internalWebClient.AddAuthorizationHeader(token.Type, token.Value);
        }

        public void Dispose()
        {
            _internalWebClient.RemoveAuthorizationHeader();
            _internalWebClient.Dispose();
        }

        public Task<string> GetAsync(string path)
        {
            return _internalWebClient.GetStringAsync(ServerEnvironment.ToUri(path));
        }

        public Task<TResponse> GetFastJsonAsync<TResponse>(string path)
        {
            return _internalWebClient.GetFromFastJsonPolymorphicAsync<TResponse>(ServerEnvironment.ToUri(path));
        }

        public Task<TResponse> GetAsync<TResponse>(string path)
        {
            return _internalWebClient.GetFromJsonAsync<TResponse>(ServerEnvironment.ToUri(path));
        }

        public Task<string> PostAsync(string path, Dictionary<string, string> form)
        {
            return _internalWebClient.PostAsync(ServerEnvironment.ToUri(path), form);
        }

        public Task<string> PostAsync(string path)
        {
            return _internalWebClient.PostAsync(ServerEnvironment.ToUri(path));
        }

        public async Task<TResponse> PostFastJsonAsync<TResponse>(string path, Dictionary<string, string> form)
        {
            return await _internalWebClient.PostAsFastJsonPolymorphicAsync<TResponse>(ServerEnvironment.ToUri(path), form);
        }

        public async Task<TResponse> PostAsync<TResponse>(string path, Dictionary<string, string> form)
        {
            return await _internalWebClient.PostAsFastJsonPolymorphicAsync<TResponse>(ServerEnvironment.ToUri(path), form);
        }
    }
}