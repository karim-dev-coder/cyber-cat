using System.Text;
using ApiGateway.Client.Models;
using Bonsai;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

[BonsaiNode("CodeEditor/")]
public class OpenCodeEditor : AsyncTask
{
    [SerializeField] private TaskType _taskType;

    private ITask _task;
    private ICodeEditor _codeEditor;

    [Inject]
    private async void Construct(AsyncInject<IPlayer> playerAsync)
    {
        var player = await playerAsync;
        _task = player.Tasks[_taskType.GetId()];
    }

    protected override async UniTask OnEnterAsync()
    {
        _codeEditor = await CodeEditor.OpenAsync(_task);
    }

    protected override UniTask<Status> RunAsync()
    {
        if (!_codeEditor.IsOpen)
        {
            return UniTask.FromResult(Status.Success);
        }

        return UniTask.FromResult(Status.Running);
    }

    public override void Description(StringBuilder builder)
    {
        base.Description(builder);
        builder.AppendLine($"Task: {_taskType}");
    }
}