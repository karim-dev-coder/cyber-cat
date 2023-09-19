using System.Threading.Tasks;
using ApiGateway.Client.Models;

namespace ApiGateway.Client.Internal.Serverless
{
    internal class ServerlessPlayer : IPlayer
    {
        public ITaskRepository Tasks => TaskRepositoryServerless.GetOrCreate();

        public Task Remove()
        {
            return Task.CompletedTask;
        }
    }
}