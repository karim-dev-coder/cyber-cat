using UnityEngine;

public class CodeEditorStartup : MonoBehaviour
{
    [SerializeField] private CodeEditorController codeEditor;
    [SerializeField] private CodeEditorContainer container;

    public void SetupCodeEditorForTask(ITaskTicket task)
    {
        codeEditor.SetupCodeEditor(task);
        container.SetTaskDescription(task);
    }
}