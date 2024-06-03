using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;
    [HideInInspector] public bool isConnectionPossible;

    public DialogChangeCondition[] _connectionChangeConditions = new DialogChangeCondition[0];

    [HideInInspector] public DialogNode outputNode;
    [HideInInspector] public DialogNode inputNode;

    [HideInInspector] public Dialog prevOutputDialog;
    [HideInInspector] public Dialog prevInputDialog;

    public void UpdateDialogs()
    {
        if(isConnectionPossible)
        {
            //reboot previous dialog transitions
            foreach (DialogChangeCondition changeCondition in _connectionChangeConditions)
                prevOutputDialog.possibleNextDialogs[prevInputDialog].Remove(changeCondition);

            if (prevOutputDialog.possibleNextDialogs[prevInputDialog].Count <= 0)
                prevOutputDialog.possibleNextDialogs.Remove(prevInputDialog);

            _connectionChangeConditions = new DialogChangeCondition[0];
        }

        if (inputNode.Dialog == null || outputNode.Dialog == null)
        {
            isConnectionPossible = false;
            return;
        }

        prevOutputDialog = outputNode.Dialog;
        prevInputDialog = inputNode.Dialog;

        isConnectionPossible = true;

        connectionName = outputNode.View.title + "->" + inputNode.View.title;

        if (!outputNode.Dialog.possibleNextDialogs.ContainsKey(inputNode.Dialog))
            outputNode.Dialog.possibleNextDialogs.Add(inputNode.Dialog, new List<DialogChangeCondition>());

    }

    public void Update()
    {
        List<DialogChangeCondition> nodeChangeConditions = new List<DialogChangeCondition>();
        outputNode.Dialog.possibleNextDialogs[inputNode.Dialog].ForEach(condition => nodeChangeConditions.Add(condition));

        //check for missing values in Dialog.possibleNextDialogs
        foreach (DialogChangeCondition edgeChangeCondition in _connectionChangeConditions)
            if (!nodeChangeConditions.Contains(edgeChangeCondition))
                nodeChangeConditions.Add(edgeChangeCondition);

        //check for extra values in Dialog.possibleNextDialogs
        foreach (DialogChangeCondition dialogChangeCondition in outputNode.Dialog.possibleNextDialogs[inputNode.Dialog])
            if (!_connectionChangeConditions.Contains(dialogChangeCondition))
                nodeChangeConditions.Remove(dialogChangeCondition);

        outputNode.Dialog.possibleNextDialogs[inputNode.Dialog] = nodeChangeConditions;
    }
}

[CustomEditor(typeof(DialogConnection))]
class DialogConnectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DialogConnection dialogConnection = (DialogConnection)target;

        EditorGUILayout.LabelField(dialogConnection.isConnectionPossible ? dialogConnection.connectionName : "Both nodes must have a Dialog to create a connection");

        if (!dialogConnection.isConnectionPossible)
            return;

        base.OnInspectorGUI();
    }
}