namespace TaskUnits.TaskDataModels
{
    internal class EmptyTaskData : ITaskData
    {
        public string Id => "undefined";
        public string Name => "undefined";
        public string Description => "undefined";
        public string Output => "undefined";
        public bool? IsSolved => null;
        public float ReceivedScore => 0;
        public float TotalScore => 0;
    }
}