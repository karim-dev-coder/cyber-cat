using JudgeService.Domain;
using Shared.Models.Domain.Verdicts;
using Shared.Models.Domain.Verdicts.TestCases;
using Shared.Server.Application.Services;

namespace JudgeService.Application;

public class JudgeGrpcService : IJudgeService
{
    private readonly ICodeLauncherService _codeLauncherService;
    private readonly ITaskService _taskService;
    private readonly Judge _judge = new Judge();

    public JudgeGrpcService(ICodeLauncherService codeLauncherService, ITaskService taskService)
    {
        _codeLauncherService = codeLauncherService;
        _taskService = taskService;
    }

    public async Task<Verdict> GetVerdict(GetVerdictArgs args)
    {
        var (taskId, solution) = args;
        var tests = await _taskService.GetTestCases(taskId);

        var testsVerdict = new TestCasesVerdict();
        foreach (var test in tests)
        {
            var output = await _codeLauncherService.Launch(new LaunchCodeArgs(solution, test.Inputs));
            if (!output.Success)
            {
                return new NativeFailure()
                {
                    TaskId = taskId,
                    TestCases = testsVerdict,
                    Solution = solution,
                    Error = output.StandardError,
                    DateTime = DateTime.UtcNow
                };
            }

            if (_judge.IsSuccess(output.StandardOutput, test))
                testsVerdict.AddSuccess(test, output.StandardOutput);
            else
                testsVerdict.AddFailure(test, output.StandardOutput);
        }

        return new Verdict()
        {
            TaskId = taskId,
            Solution = solution,
            TestCases = testsVerdict,
            DateTime = DateTime.UtcNow
        };
    }
}