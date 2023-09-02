using ApiGateway.Client.Models;
using Shared.Models.Dto.Data;

namespace ApiGateway.Client.Internal.Tasks.Statuses
{
    public class Complete : ITaskProgressStatus
    {
        public string Solution { get; }

        public Complete(TaskData data)
        {
            Solution = data.Solution;
        }
    }
}