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

    public async Task<GetTaskResponse> GetTask(StringProto taskId)
    {
        var task = await _taskRepository.GetTask(taskId);
        return new GetTaskResponse
        {
            Name = task.Name,
            Description = task.Description
        };
    }
}