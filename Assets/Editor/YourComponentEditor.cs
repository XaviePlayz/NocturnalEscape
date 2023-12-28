using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueTrigger))]
public class YourComponentEditor : Editor
{
    SerializedProperty hasAssociatedTaskSlot1Prop;
    SerializedProperty associatedTaskSlot1Prop;
    SerializedProperty removeTaskSlot1OnInteract;
    SerializedProperty newTaskSlot1DialogueTrigger;

    SerializedProperty hasAssociatedTaskSlot2Prop;
    SerializedProperty associatedTaskSlot2Prop;
    SerializedProperty removeTaskSlot2OnInteract;
    SerializedProperty newTaskSlot2DialogueTrigger;

    SerializedProperty hasAssociatedTaskSlot3Prop;
    SerializedProperty associatedTaskSlot3Prop;
    SerializedProperty removeTaskSlot3OnInteract;
    SerializedProperty newTaskSlot3DialogueTrigger;

    void OnEnable()
    {
        // Fetch the serialized properties
        hasAssociatedTaskSlot1Prop = serializedObject.FindProperty("hasAssociatedTaskSlot1");
        associatedTaskSlot1Prop = serializedObject.FindProperty("associatedTaskSlot1");
        removeTaskSlot1OnInteract = serializedObject.FindProperty("removeTaskSlot1OnInteract");
        newTaskSlot1DialogueTrigger = serializedObject.FindProperty("newTaskSlot1DialogueTrigger");

        hasAssociatedTaskSlot2Prop = serializedObject.FindProperty("hasAssociatedTaskSlot2");
        associatedTaskSlot2Prop = serializedObject.FindProperty("associatedTaskSlot2");
        removeTaskSlot2OnInteract = serializedObject.FindProperty("removeTaskSlot2OnInteract");
        newTaskSlot2DialogueTrigger = serializedObject.FindProperty("newTaskSlot2DialogueTrigger");

        hasAssociatedTaskSlot3Prop = serializedObject.FindProperty("hasAssociatedTaskSlot3");
        associatedTaskSlot3Prop = serializedObject.FindProperty("associatedTaskSlot3");
        removeTaskSlot3OnInteract = serializedObject.FindProperty("removeTaskSlot3OnInteract");
        newTaskSlot3DialogueTrigger = serializedObject.FindProperty("newTaskSlot3DialogueTrigger");

    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Draw the default inspector excluding associatedTaskSlot properties
        DrawPropertiesExcluding(serializedObject, "associatedTaskSlot1", "associatedTaskSlot2", "associatedTaskSlot3", "removeTaskSlot1OnInteract", "removeTaskSlot2OnInteract", "removeTaskSlot3OnInteract", "newTaskSlot1DialogueTrigger", "newTaskSlot2DialogueTrigger", "newTaskSlot3DialogueTrigger");

        // Show the associatedTaskSlot properties based on their respective hasAssociatedTaskSlot properties
        if (hasAssociatedTaskSlot1Prop.boolValue)
        {
            EditorGUILayout.PropertyField(associatedTaskSlot1Prop);
            EditorGUILayout.PropertyField(removeTaskSlot1OnInteract);
            EditorGUILayout.PropertyField(newTaskSlot1DialogueTrigger);
        }

        if (hasAssociatedTaskSlot2Prop.boolValue)
        {
            EditorGUILayout.PropertyField(associatedTaskSlot2Prop);
            EditorGUILayout.PropertyField(removeTaskSlot2OnInteract);
            EditorGUILayout.PropertyField(newTaskSlot2DialogueTrigger);
        }

        if (hasAssociatedTaskSlot3Prop.boolValue)
        {
            EditorGUILayout.PropertyField(associatedTaskSlot3Prop);
            EditorGUILayout.PropertyField(removeTaskSlot3OnInteract);
            EditorGUILayout.PropertyField(newTaskSlot3DialogueTrigger);
        }

        // Apply any changes to the serialized object
        serializedObject.ApplyModifiedProperties();
    }
}