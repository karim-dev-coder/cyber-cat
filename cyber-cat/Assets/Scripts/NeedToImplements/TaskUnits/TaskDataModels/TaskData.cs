using System;
using Newtonsoft.Json;
using Shared.Models.Dto;
using Shared.Models.Models;

namespace TaskUnits.TaskDataModels
{
    internal class TaskData : ITaskData
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("desc")] public string Description { get; set; }
        [JsonProperty("output")] public string Output { get; set; }
        [JsonProperty("score")] public float TotalScore { get; set; }

        [JsonProperty("completion")] private float _completion;

        public float ReceivedScore => _completion * TotalScore;
        public bool? IsSolved => _completion >= 1f;

        // TODO:
        [Obsolete("Не используйте его, он будет удален. Он нужен только для совеместимости старой ITaskData с новой сервисной архитекутрой ITask")]
        public static ITaskData ConvertFrom(TaskDto task)
        {
            return new TaskData
            {
                Name = task.Name,
                Description = task.Description
            };
        }
    }
}