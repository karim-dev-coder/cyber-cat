using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Models;
using Proyecto26;
using ServerAPI.InternalDto;
using UnityEngine;

namespace ServerAPI.ServerAPIImplements
{
    public class ServerAPI : IServerAPI
    {
        private readonly string _url;

        public ServerAPI(string url)
        {
            _url = url;
        }

        public async UniTask<ITokenSession> Authenticate(string email, string password, IProgress<float> progress = null)
        {
            var request = new RequestHelper
            {
                Uri = _url + "/auth/login",
                SimpleForm = new Dictionary<string, string>
                {
                    ["email"] = email,
                    ["password"] = password
                },
                ProgressCallback = value => progress?.Report(value),
                EnableDebug = Debug.isDebugBuild
            };

            var token = await RestClient.Post<TokenSessionDto>(request).ToUniTask();
            return token;
        }

        public async UniTask<IPlayer> AuthorizePlayer(ITokenSession token, IProgress<float> progress = null)
        {
            RestClient.DefaultRequestHeaders["Authorization"] = $"Bearer {token.Value}";

            var request = new RequestHelper
            {
                Uri = _url + "/auth/authorize_player",
                ProgressCallback = value => progress?.Report(value),
                EnableDebug = Debug.isDebugBuild
            };

            var response = await RestClient.Get(request).ToUniTask();
            return new PlayerDto
            {
                Name = response.Text,
                Token = token
            };
        }

        public async UniTask<ITask> GetTask(string taskId, IProgress<float> progress = null)
        {
            var request = new RequestHelper
            {
                Uri = _url + $"/tasks/{taskId}",
                ProgressCallback = value => progress?.Report(value),
                EnableDebug = Debug.isDebugBuild
            };

            var task = await RestClient.Get<TaskDto>(request).ToUniTask();
            return task;
        }
    }
}