using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogNode : ScriptableObject
{
    bool isInitial;

    [SerializeField] DialogData dialogData;
    Dialog prevDialog;
    DialogNodeView dialogNodeView;
    DialogConnection inputConnection;
    List<DialogConnection> outputConnections = new List<DialogConnection>();

    public bool IsInitial{ set { isInitial = value; } get{ return isInitial; } }

    public DialogData DialogData { set { dialogData = value; } get{ return dialogData; } }
    public Dialog PrevDialog { set { prevDialog = value; } get{ return prevDialog; } }
    public DialogNodeView View{ set { dialogNodeView = value; } get{ return dialogNodeView; } }
    public DialogConnection InputConnection { set { inputConnection = value; } get{ return inputConnection; } }
    public List<DialogConnection> OutputConnections { set { outputConnections = value; } get{ return outputConnections; } }

    //in condition
    //out conditions[]
}

class DialogNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogNode dialogNode = (DialogNode)target;

        base.OnInspectorGUI();

        EditorGUILayout.LabelField("Dialog Stage: ");
    }
}