using System;
using Cysharp.Threading.Tasks;
using Models;

namespace ServerAPI
{
    public interface IServerAPI
    {
        UniTask<ITokenSession> Authenticate(string email, string password, IProgress<float> progress = null);
        UniTask<IPlayer> AuthorizePlayer(ITokenSession token, IProgress<float> progress = null);
        UniTask<ITask> GetTask(string taskId, IProgress<float> progress = null);
    }
}