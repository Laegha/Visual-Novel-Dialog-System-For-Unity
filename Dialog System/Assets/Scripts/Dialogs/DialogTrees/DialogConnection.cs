using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;

    public DialogChangeCondition[] dialogChangeConditions;

    [HideInInspector] public DialogNode inputDialog;
    [HideInInspector] public DialogNode outputDialog;
}

[CustomEditor(typeof(DialogConnection))]
class DialogConnectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogConnection dialogConnection = (DialogConnection)target;

        EditorGUILayout.LabelField(dialogConnection.connectionName);

        base.OnInspectorGUI();
    }
}