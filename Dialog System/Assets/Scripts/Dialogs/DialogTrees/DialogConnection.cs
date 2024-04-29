using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;
    [HideInInspector] public bool isConnectionPossible;
    [HideInInspector] public bool isLoop;

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
            if (isLoop)
                foreach (DialogChangeCondition changeCondition in dialogChangeConditions)
                    prevOutputDialog.loopConditions.Remove(changeCondition);
            else
                prevOutputDialog.possibleNextDialogs.Remove(prevInputDialog);

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

        if (outputNode.Dialog == inputNode.Dialog)
            isLoop = true;
        else
            isLoop = false;
    }

    public void Update()
    {
        List<DialogChangeCondition> dialogChangeConditions = isLoop ? outputNode.Dialog.loopConditions : outputNode.Dialog.possibleNextDialogs[inputNode.Dialog].ToList();

        //check for missing values in Dialog.loopConditions/possibleNextDialogs
        foreach (DialogChangeCondition edgeChangeCondition in this.dialogChangeConditions)
            if (!dialogChangeConditions.Contains(edgeChangeCondition))
                dialogChangeConditions.Add(edgeChangeCondition);

        //check for extra values in Dialog.loopConditions/possibleNextDialogs
        foreach (DialogChangeCondition dialogChangeCondition in isLoop ? outputNode.Dialog.loopConditions.ToArray() : outputNode.Dialog.possibleNextDialogs[inputNode.Dialog])
            if (!this.dialogChangeConditions.Contains(dialogChangeCondition))
                dialogChangeConditions.Remove(dialogChangeCondition);

        if (isLoop)
            outputNode.Dialog.loopConditions = dialogChangeConditions;
        else
            outputNode.Dialog.possibleNextDialogs[inputNode.Dialog] = dialogChangeConditions.ToArray();
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