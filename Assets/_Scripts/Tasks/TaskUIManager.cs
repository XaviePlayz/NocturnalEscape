using UnityEngine;
using UnityEngine.UI;

public class TaskUIManager : MonoBehaviour
{
    public Text taskText; // Assign the UI Text element in the Inspector

    // Update is called once per frame
    void Update()
    {
        // Clear the task text
        taskText.text = "";

        // Display each active task
        foreach (TaskData task in TaskManager.Instance.tasks)
        {
            taskText.text += task.description + "\n";
        }
    }
}