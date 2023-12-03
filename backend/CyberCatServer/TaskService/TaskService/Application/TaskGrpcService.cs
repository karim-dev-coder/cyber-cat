using Shared.Models.Domain.Tasks;
using Shared.Models.Domain.TestCase;
using Shared.Server.Data;
using Shared.Server.ExternalData;
using Shared.Server.Services;
using TaskService.Infrastructure;

namespace TaskService.Application;

public class TaskGrpcService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ISharedTaskProgressRepository _sharedTaskProgressRepository;
    private readonly SharedTaskWebHookProcessor _taskWebHookProcessor;
    private readonly TaskEntityMapper _taskEntityMapper;

    public TaskGrpcService(ITaskRepository taskRepository, ISharedTaskProgressRepository sharedTaskProgressRepository,
        SharedTaskWebHookProcessor taskWebHookProcessor, TaskEntityMapper taskEntityMapper)
    {
        _taskEntityMapper = taskEntityMapper;
        _taskWebHookProcessor = taskWebHookProcessor;
        _taskRepository = taskRepository;
        _sharedTaskProgressRepository = sharedTaskProgressRepository;
    }

    public async Task<List<TaskDescription>> GetTasks()
    {
        var result = new List<TaskDescription>();
        var tasks = await _taskRepository.GetTasks();

        foreach (var task in tasks)
        {
            var description = await _taskEntityMapper.ToDescription(task);
            result.Add(description);
        }

        return result;
    }

    public async Task<List<TestCaseDescription>> GetTestCases(TaskId taskId)
    {
        var task = await _taskRepository.GetTask(taskId);
        var descriptions = _taskEntityMapper.ToTestCaseDescriptions(task);
        return descriptions;
    }

    public async Task OnTaskSolved(OnTaskSolvedArgs args)
    {
        var sharedTask = await _sharedTaskProgressRepository.GetTask(args.TaskId);
        if (sharedTask == null || sharedTask.Status == SharedTaskStatus.NotSolved)
        {
            sharedTask = await _sharedTaskProgressRepository.SetSolved(args.TaskId, args.UserId);
            await _taskWebHookProcessor.ProcessWebHook(sharedTask);
        }
    }

    public async Task<List<SharedTaskProgressData>> GetSharedTasks()
    {
        var tasks = await _sharedTaskProgressRepository.GetTasks();
        return tasks;
    }

    public async Task<WebHookResultStatus> ProcessWebHookTest()
    {
        return await _taskWebHookProcessor.ProcessWebHook(SharedTaskExternalDto.Mock(true));
    }
}