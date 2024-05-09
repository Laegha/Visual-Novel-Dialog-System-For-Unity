using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;
    [HideInInspector] public bool isConnectionPossible;

    public DialogChangeCondition[] dialogChangeConditions = new DialogChangeCondition[0];

    [HideInInspector] public DialogNode outputNode;
    [HideInInspector] public DialogNode inputNode;

    [HideInInspector] public Dialog prevOutputDialog;
    [HideInInspector] public Dialog prevInputDialog;

    public void UpdateDialogs()
    {
        if(isConnectionPossible)
        {
            //reboot previous dialog transitions
            foreach (DialogChangeCondition changeCondition in dialogChangeConditions)
                prevOutputDialog.possibleNextDialogs[prevInputDialog].Remove(changeCondition);

            dialogChangeConditions = new DialogChangeCondition[0];
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

    }

    public void Update()
    {
        List<DialogChangeCondition> dialogChangeConditions = new List<DialogChangeCondition>();
        outputNode.Dialog.possibleNextDialogs[inputNode.Dialog].ForEach(condition => dialogChangeConditions.Add(condition));

        //check for missing values in Dialog.loopConditions/possibleNextDialogs
        foreach (DialogChangeCondition edgeChangeCondition in this.dialogChangeConditions)
            if (!dialogChangeConditions.Contains(edgeChangeCondition))
                dialogChangeConditions.Add(edgeChangeCondition);

        //check for extra values in Dialog.loopConditions/possibleNextDialogs
        foreach (DialogChangeCondition dialogChangeCondition in outputNode.Dialog.possibleNextDialogs[inputNode.Dialog])
            if (!this.dialogChangeConditions.Contains(dialogChangeCondition))
                dialogChangeConditions.Remove(dialogChangeCondition);

        outputNode.Dialog.possibleNextDialogs[inputNode.Dialog] = dialogChangeConditions;
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