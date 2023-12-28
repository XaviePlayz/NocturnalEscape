using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton

    private static DialogueManager _instance;
    public static DialogueManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DialogueManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(DialogueManager).Name;
                    _instance = obj.AddComponent<DialogueManager>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public Text dialogueText;

    private Queue<string> dialogueQueue;
    private bool isDialogueActive;
    private bool isPressToContinue;
    private bool isAutoDisplaying;
    private bool isFirstLine;

    private PlayerController playerController;
    private DialogueTrigger currentDialogueTrigger;

    void Awake()
    {
        dialogueQueue = new Queue<string>();
        playerController = FindObjectOfType<PlayerController>();
    }

    public void StartDialogue(DialogueTrigger dialogueTrigger, DialogueData dialogueData, bool pressToContinue)
    {
        if (!isDialogueActive && dialogueTrigger.isTriggerable)
        {
            isDialogueActive = true;

            // Clear previous dialogue
            dialogueQueue.Clear();

            // Enqueue new lines of dialogue
            foreach (string line in dialogueData.lines)
            {
                dialogueQueue.Enqueue(line);
            }

            // Set the boolean for "Press E" dialogue
            isPressToContinue = pressToContinue;

            // Set the current DialogueTrigger
            currentDialogueTrigger = dialogueTrigger;

            // Freeze player position if needed
            if (pressToContinue)
            {
                playerController.FreezePlayer();
                isFirstLine = true;
            }
            else
            {
                // Start displaying dialogue
                StartCoroutine(DisplayLines());
            }
        }
    }

    public void Update()
    {
        // Check for first line of dialogue
        if (isFirstLine)
        {
            isFirstLine = false;
            string line = dialogueQueue.Dequeue();
            dialogueText.text = line;
        }

        // Check for key press to advance dialogue for "Press E" dialogue
        if (isDialogueActive && isPressToContinue && Input.GetKeyDown(KeyCode.E))
        {

            if (!isAutoDisplaying)
            {
                DisplayNextLine();
            }
        }
    }

    public void DisplayNextLine()
    {
        if (dialogueQueue.Count > 0)
        {
            string line = dialogueQueue.Dequeue();
            dialogueText.text = line;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        ActivateDialogueTrigger1(currentDialogueTrigger);
        isDialogueActive = false;
        playerController.inDialogue = false;

        dialogueQueue.Clear();
        dialogueText.text = "";

        HandleTaskSlots();

        playerController.UnFreezePlayer();
    }

    private void HandleTaskSlots()
    {
        if (!currentDialogueTrigger.hasInteractedAfterDialogue)
        {
            if (currentDialogueTrigger.removeTaskSlot1OnInteract)
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot1)
                {
                    TaskManager.Instance.CompleteTask(currentDialogueTrigger.associatedTaskSlot1);
                    Debug.Log("Task completed: " + currentDialogueTrigger.associatedTaskSlot1.description);
                }               
            }
            else
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot1)
                {
                    TaskManager.Instance.AddTask(currentDialogueTrigger.associatedTaskSlot1);
                    Debug.Log("Task added: " + currentDialogueTrigger.associatedTaskSlot1.description);
                }              
            }

            if (currentDialogueTrigger.removeTaskSlot2OnInteract)
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot2)
                {
                    TaskManager.Instance.CompleteTask(currentDialogueTrigger.associatedTaskSlot2);
                    Debug.Log("Task completed: " + currentDialogueTrigger.associatedTaskSlot2.description);
                }
            }
            else
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot2)
                {
                    TaskManager.Instance.AddTask(currentDialogueTrigger.associatedTaskSlot2);
                    Debug.Log("Task added: " + currentDialogueTrigger.associatedTaskSlot2.description);
                }
            }

            if (currentDialogueTrigger.removeTaskSlot3OnInteract)
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot3)
                {
                    TaskManager.Instance.CompleteTask(currentDialogueTrigger.associatedTaskSlot3);
                    Debug.Log("Task completed: " + currentDialogueTrigger.associatedTaskSlot3.description);
                }
            }
            else
            {
                if (currentDialogueTrigger.hasAssociatedTaskSlot3)
                {
                    TaskManager.Instance.AddTask(currentDialogueTrigger.associatedTaskSlot3);
                    Debug.Log("Task added: " + currentDialogueTrigger.associatedTaskSlot3.description);
                }
                currentDialogueTrigger.hasInteractedAfterDialogue = true;
            }                                 
        }
    }

    public void ActivateDialogueTrigger1(DialogueTrigger dialogueTrigger)
    {
        if (currentDialogueTrigger.newTaskSlot1DialogueTrigger != null)
        {
            dialogueTrigger.newTaskSlot1DialogueTrigger.SetTriggerable(true);
        }
        if (currentDialogueTrigger.newTaskSlot2DialogueTrigger != null)
        {
            dialogueTrigger.newTaskSlot2DialogueTrigger.SetTriggerable(true);
        }
        if (currentDialogueTrigger.newTaskSlot3DialogueTrigger != null)
        {
            dialogueTrigger.newTaskSlot3DialogueTrigger.SetTriggerable(true);
        }
    }

    private IEnumerator DisplayLines()
    {
        while (dialogueQueue.Count > 0)
        {
            string line = dialogueQueue.Dequeue();
            dialogueText.text = line;

            if (isPressToContinue)
            {
                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
            }
            else
            {
                yield return new WaitForSeconds(currentDialogueTrigger.timeToContinue);
            }
        }

        EndDialogue();
    }
}