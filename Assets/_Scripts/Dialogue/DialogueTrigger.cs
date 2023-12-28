using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public DialogueData dialogue;
    public bool isTriggerable = true;
    public bool pressToContinue;
    public int timeToContinue;

    [Header("Interacted")]
    public bool hasInteracted = false;
    public bool hasInteractedAfterDialogue = false;

    [Header("Manage Task Slots")]
    public bool hasAssociatedTaskSlot1;
    public TaskData associatedTaskSlot1;
    public bool removeTaskSlot1OnInteract = false;
    public DialogueTrigger newTaskSlot1DialogueTrigger;

    public bool hasAssociatedTaskSlot2;
    public TaskData associatedTaskSlot2;
    public bool removeTaskSlot2OnInteract = false;
    public DialogueTrigger newTaskSlot2DialogueTrigger;

    public bool hasAssociatedTaskSlot3;
    public TaskData associatedTaskSlot3;
    public bool removeTaskSlot3OnInteract = false;
    public DialogueTrigger newTaskSlot3DialogueTrigger;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasInteracted && IsTriggerable())
        {
            DialogueManager.Instance.StartDialogue(this, dialogue, pressToContinue);
            hasInteracted = true;
        }
    }
    public void SetTriggerable(bool value)
    {
        isTriggerable = value;
    }

    public bool IsTriggerable()
    {
        return isTriggerable;
    }
}