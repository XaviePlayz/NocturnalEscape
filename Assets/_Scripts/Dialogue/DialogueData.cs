using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue Data", order = 1)]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] lines;
}