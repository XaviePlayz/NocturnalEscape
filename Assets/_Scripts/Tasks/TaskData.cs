using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Game/Task Data", order = 1)]
public class TaskData : ScriptableObject
{
    [TextArea(3, 10)]
    public string description = "Task Description";

    public TaskState currentState = TaskState.Inactive;

    public bool isComplete => currentState == TaskState.Complete;
}

public enum TaskState
{
    Inactive,
    Active,
    Complete
}
