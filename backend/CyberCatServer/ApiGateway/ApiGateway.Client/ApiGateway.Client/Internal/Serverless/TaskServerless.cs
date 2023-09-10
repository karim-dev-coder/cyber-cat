using System.Threading.Tasks;
using ApiGateway.Client.Internal.Tasks.Statuses;
using ApiGateway.Client.Internal.Tasks.Verdicts;
using ApiGateway.Client.Models;
using Shared.Models.Dto.Data;
using Shared.Models.Enums;

namespace ApiGateway.Client.Internal.Serverless
{
    internal class TaskServerless : ITask
    {
        private int _verifyCount;
        private string _solution;

        public Task<string> GetName()
        {
            return Task.FromResult("Обучение. Режим без сервера.");
        }

        public Task<string> GetDescription()
        {
            return Task.FromResult("Вы играете в режиме без сервера. Первая проверка задачи - всегда ошибка. Вторая проверка - всегда успех.");
        }

        public Task<ITaskProgressStatus> GetStatus()
        {
            switch (_verifyCount)
            {
                case 0:
                    return Task.FromResult<ITaskProgressStatus>(new NotStarted());
                case 1:
                    return Task.FromResult<ITaskProgressStatus>(new HaveSolution(_solution));
                default:
                    return Task.FromResult<ITaskProgressStatus>(new Complete(_solution));
            }
        }

        public Task<IVerdict> VerifySolution(string sourceCode)
        {
            _solution = sourceCode;
            _verifyCount++;

            switch (_verifyCount)
            {
                case 1:
                    return Task.FromResult<IVerdict>(new Failure(new VerdictData()
                    {
                        Error = "Это режим без сервера. Отправьте задачу на проверку ещё раз, чтобы решение было успешным.",
                        Status = VerdictStatus.Failure
                    }));

                default:
                    return Task.FromResult<IVerdict>(new Success(new VerdictData()
                    {
                        Status = VerdictStatus.Success
                    }));
            }
        }
    }
}