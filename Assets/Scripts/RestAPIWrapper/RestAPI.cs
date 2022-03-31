using Authentication;
using Cysharp.Threading.Tasks;
using Extensions.RestClientExt;
using Proyecto26;

namespace RestAPIWrapper
{
    public static class RestAPI
    {
        public static async UniTask<TokenSession> GetToken(string login, string password)
        {
            var request = new RequestHelper
            {
                Uri = Endpoint.LOGIN,
                Params =
                {
                    ["email"] = login,
                    ["pass"] = password
                }
            };

            return await RestClient.Get<TokenSession>(request).ToUniTask();
        }

        public static async UniTask<TokenSession> Registrate(string login, string password, string name)
        {
            var request = new RequestHelper
            {
                Uri = Endpoint.REGISTER,
                Params =
                {
                    ["email"] = login,
                    ["pass"] = password,
                    ["name"] = name
                }
            };

            return await RestClient.Post<TokenSession>(request).ToUniTask();
        }
    }
}