﻿using System.Net;
using PlayerService.Domain;
using Shared.Models.Domain.Tasks;
using Shared.Models.Domain.Verdicts;
using Shared.Server.Exceptions;
using Shared.Server.ProtoHelpers;
using Shared.Server.Services;

namespace PlayerService.Application;

public class PlayerGrpcService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IJudgeService _judgeService;
    private readonly ITaskService _taskService;
    private readonly ILogger<PlayerGrpcService> _logger;

    public PlayerGrpcService(IPlayerRepository playerRepository, IJudgeService judgeService, ITaskService taskService, ILogger<PlayerGrpcService> logger)
    {
        _logger = logger;
        _taskService = taskService;
        _judgeService = judgeService;
        _playerRepository = playerRepository;
    }

    public async Task<Response<List<TaskProgress>>> GetTasksProgress(GetTasksProgressArgs args)
    {
        var player = await _playerRepository.GetPlayer(args.UserId);
        if (player == null)
            return new List<TaskProgress>();

        var progress = player.Tasks.Values.Select(progress => progress.ToDomain()).ToList();
        return progress;
    }

    public async Task<Response<TaskProgress>> GetTaskProgress(GetTaskProgressArgs args)
    {
        var player = await _playerRepository.GetPlayer(args.UserId);
        if (player == null)
        {
            return new TaskProgress()
            {
                TaskId = args.TaskId
            };
        }

        return player.Tasks.GetValueOrDefault(args.TaskId)?.ToDomain();
    }

    public async Task<Response<Verdict>> SubmitSolution(SubmitSolutionArgs args)
    {
        var (userId, taskId, solution) = args;
        var result = await _judgeService.GetVerdict(args);
        if (!result.IsSucceeded)
        {
            return result;
        }

        var verdict = result.Value;
        _logger.LogInformation("{Task} verdict: {Verdict}. Player '{UserId}'", args.TaskId, verdict.ToString(), userId);

        var player = await _playerRepository.GetPlayer(userId);
        if (player == null)
        {
            var createResult = await _playerRepository.Create(userId);
            if (!createResult.IsSuccess)
            {
                switch (createResult.Error)
                {
                    default:
                        throw new ServiceException("Неизвестная ошибка. Обратитесь к администратору", HttpStatusCode.BadRequest);
                }
            }

            player = createResult.Player;
        }

        if (!player.Tasks.ContainsKey(taskId))
            player.Tasks[taskId] = new TaskProgressEntity();

        player.Tasks[taskId].Solution = solution;

        switch (verdict)
        {
            case Success success:
                player.Tasks[taskId].StatusType = TaskProgressStatusType.Complete;
                await _taskService.OnTaskSolved(new OnTaskSolvedArgs(userId, taskId));
                break;
            default:
                player.Tasks[taskId].StatusType = TaskProgressStatusType.HaveSolution;
                break;
        }

        var updateResult = await _playerRepository.Update(player);
        if (!updateResult.IsSuccess)
        {
            switch (updateResult.Error)
            {
                default:
                    throw new ServiceException("Неизвестная ошибка. Обратитесь к администратору", HttpStatusCode.BadRequest);
            }
        }

        return verdict;
    }

    public async Task<Response> RemovePlayer(RemovePlayerArgs args)
    {
        var result = await _playerRepository.Delete(args.UserId);
        if (!result.IsSuccess)
        {
            switch (result.Error)
            {
                case UserRepositoryError.NotFound:
                    // Already deleted. ¯\_(ツ)_/¯
                    break;
                default:
                    throw new ServiceException("Неизвестная ошибка. Обратитесь к администратору", HttpStatusCode.BadRequest);
            }
        }

        return new Response();
    }
}