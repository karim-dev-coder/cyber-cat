﻿using Shared.Models.Data;
using Shared.Models.Domain.Players;
using Shared.Models.Domain.Tasks;
using Shared.Models.Domain.Users;
using Shared.Models.Enums;
using Shared.Models.Ids;

namespace PlayerService.Repositories;

public interface IPlayerRepository
{
    Task<PlayerId> CreatePlayer(UserId playerId);
    Task<PlayerId> GetPlayerByUserId(UserId userId);
    Task RemovePlayer(PlayerId playerId);
    Task<PlayerData> GetPlayerById(PlayerId playerId);
    Task AddBitcoins(PlayerId playerId, int bitcoins);
    Task RemoveBitcoins(PlayerId playerId, int bitcoins);
    Task SetTaskStatus(PlayerId playerId, TaskId taskId, TaskProgressStatus status);
    Task<TaskProgressData> GetTaskData(PlayerId playerId, TaskId taskId);
    Task SaveCode(PlayerId playerId, TaskId taskId, string solution);
}