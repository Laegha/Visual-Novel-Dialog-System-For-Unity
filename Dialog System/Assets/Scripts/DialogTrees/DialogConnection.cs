using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;
    [HideInInspector] public bool isConnectionPossible;

    public DialogChangeCondition[] connectionChangeConditions = new DialogChangeCondition[0];

    [HideInInspector] public DialogNode outputNode;
    [HideInInspector] public DialogNode inputNode;

    [HideInInspector] public DialogData prevOutputDialog;
    [HideInInspector] public DialogData prevInputDialog;

    #region Managing changes between dialogs
    public void UpdateDialogs()
    {
        if(isConnectionPossible)
        {
            //remove previous dialog transitions
            UnlinkConnection();

            connectionChangeConditions = new DialogChangeCondition[0];
        }

        if (inputNode.DialogData.Dialog == null || outputNode.DialogData.Dialog == null)
            isConnectionPossible = false;
        else
            isConnectionPossible = true;

        prevOutputDialog = outputNode.DialogData;
        prevInputDialog = inputNode.DialogData;

        connectionName = outputNode.View.title + " -> " + inputNode.View.title;

        //Debug.Log(outputNode.DialogData.PossibleNextDialogs);
        if(!outputNode.DialogData.PossibleNextDialogs.ContainsKey(inputNode.DialogData))
            outputNode.DialogData.PossibleNextDialogs.Add(inputNode.DialogData, new List<DialogChangeCondition>());

    }
    #endregion

    #region Managing changes in edge
    public void Update()
    {
        //List<DialogChangeCondition> nodeChangeConditions = new List<DialogChangeCondition>();
        //outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData].ForEach(condition => nodeChangeConditions.Add(condition));

        ////check for missing values in Dialog.possibleNextDialogs
        //foreach (DialogChangeCondition edgeChangeCondition in connectionChangeConditions)
            //if (!nodeChangeConditions.Contains(edgeChangeCondition))
                //nodeChangeConditions.Add(edgeChangeCondition);


        ////check for extra values in Dialog.possibleNextDialogs
        //foreach (DialogChangeCondition dialogChangeCondition in outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData])
            //if (!connectionChangeConditions.Contains(dialogChangeCondition))
                //nodeChangeConditions.Remove(dialogChangeCondition);

        //check for extra values in Dialog.possibleNextDialogs
        outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData].ForEach(condition =>{
            if (!connectionChangeConditions.Contains(condition))
            {
                UpdateNodeConditions();
                return;

            }
        });
        //check for missing values in Dialog.possibleNextDialogs
        foreach (var item in connectionChangeConditions)
            if (!outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData].Contains(item))
            {
                UpdateNodeConditions();
                return;
            }


    }
    #endregion

    void UpdateNodeConditions()
    {
        outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData] = connectionChangeConditions.ToList();
        Debug.Log("Updated node");

    }

    public void UnlinkConnection()
    {
        prevOutputDialog.PossibleNextDialogs.Remove(prevInputDialog);
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