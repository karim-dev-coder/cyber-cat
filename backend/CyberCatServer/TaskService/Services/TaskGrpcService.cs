using Shared.Dto;
using Shared.Services;
using TaskService.Repositories;

namespace TaskService.Services;

public class TaskGrpcService : ITaskGrpcService
{
    private readonly ITaskRepository _taskRepository;

    public TaskGrpcService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async Task<TaskResponse> GetTask(TaskIdArg arg)
    {
        var task = await _taskRepository.GetTask(arg.Id);
        return TaskResponse.FromTask(task);
    }
}