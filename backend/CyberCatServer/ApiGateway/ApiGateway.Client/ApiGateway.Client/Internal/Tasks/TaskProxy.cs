using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApiGateway.Client.Internal.Tasks.Statuses;
using ApiGateway.Client.Internal.WebClientAdapters;
using ApiGateway.Client.Models;
using Shared.Models.Domain.Tasks;
using Shared.Models.Domain.Verdicts;
using Shared.Models.Models.TestCases;

namespace ApiGateway.Client.Internal.Tasks
{
    internal class TaskProxy : ITask
    {
        private TaskDescription _description;

        private readonly TaskId _taskId;
        private readonly Uri _uri;
        private readonly IWebClientAdapter _webClient;

        public TaskProxy(TaskId taskId, Uri uri, IWebClientAdapter webClient)
        {
            _webClient = webClient;
            _uri = uri;
            _taskId = taskId;
        }

        public async Task<string> GetName()
        {
            if (_description == null)
            {
                _description = await _webClient.GetFromJsonAsync<TaskDescription>(_uri + $"tasks/{_taskId}");
            }

            return _description.Name;
        }

        public async Task<string> GetDescription()
        {
            if (_description == null)
            {
                _description = await _webClient.GetFromJsonAsync<TaskDescription>(_uri + $"tasks/{_taskId}");
            }

            return _description.Description;
        }

        public async Task<string> GetDefaultCode()
        {
            if (_description == null)
            {
                _description = await _webClient.GetFromJsonAsync<TaskDescription>(_uri + $"tasks/{_taskId}");
            }

            return _description.DefaultCode;
        }

        public async Task<ITaskProgressStatus> GetStatus()
        {
            var data = await _webClient.GetFromJsonAsync<TaskProgress>(_uri + $"player/tasks/{_taskId}");
            return GetStatus(data);
        }

        public async Task<TestCases> GetTestCases()
        {
            var testCases = await _webClient.GetFromFastJsonPolymorphicAsync<TestCases>(_uri + $"tasks/{_taskId}/tests");
            return testCases;
        }

        private ITaskProgressStatus GetStatus(TaskProgress progress)
        {
            switch (progress.StatusType)
            {
                case TaskProgressStatusType.NotStarted:
                    return new NotStarted();
                case TaskProgressStatusType.HaveSolution:
                    return new HaveSolution(progress.Solution);
                case TaskProgressStatusType.Complete:
                    return new Complete(progress.Solution);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<Verdict> VerifySolution(string sourceCode)
        {
            var verdict = await _webClient.PostAsFastJsonPolymorphicAsync<Verdict>(_uri + $"player/verify/{_taskId}", new Dictionary<string, string>
            {
                ["sourceCode"] = sourceCode
            });

            return verdict;
        }
    }
}