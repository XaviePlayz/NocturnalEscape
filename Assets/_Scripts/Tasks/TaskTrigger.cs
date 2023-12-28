using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    public TaskData associatedTask;
    public bool removeTaskOnInteract = false;

    private bool hasInteracted = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasInteracted)
        {
            if (removeTaskOnInteract)
            {
                TaskManager.Instance.CompleteTask(associatedTask);
                Debug.Log("Task completed: " + associatedTask.description);
            }
            else
            {
                TaskManager.Instance.AddTask(associatedTask);
                Debug.Log("Task added: " + associatedTask.description);
            }

            hasInteracted = true;
        }
    }
}
