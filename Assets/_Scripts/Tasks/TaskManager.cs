using UnityEngine;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    #region Singleton

    private static TaskManager _instance;
    public static TaskManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TaskManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(TaskManager).Name;
                    _instance = obj.AddComponent<TaskManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public List<TaskData> tasks = new List<TaskData>();
    public AudioClip taskAddedSound;

    public void AddTask(TaskData task)
    {
        if (!tasks.Contains(task))
        {
            tasks.Add(task);
            task.currentState = TaskState.Active;

            if (taskAddedSound != null)
            {
                AudioSource.PlayClipAtPoint(taskAddedSound, Camera.main.transform.position);
            }

            Debug.Log("Task added: " + task.description);
        }
    }

    public void CompleteTask(TaskData task)
    {
        if (tasks.Contains(task))
        {
            tasks.Remove(task);
            task.currentState = TaskState.Complete;
            Debug.Log("Task completed: " + task.description);
        }
    }
}
