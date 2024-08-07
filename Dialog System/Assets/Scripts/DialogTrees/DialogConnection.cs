using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class DialogConnection : ScriptableObject
{
    [HideInInspector] public string connectionName;
    [HideInInspector] public bool isConnectionPossible;

    public DialogChangeCondition[] connectionChangeConditions = new DialogChangeCondition[0];
    public DialogChangeCondition[] ConnectionChangeConditions { get { return connectionChangeConditions; } set { connectionChangeConditions = value; Debug.Log("connectionChangeConditions: " + connectionChangeConditions.Length); } }

    [HideInInspector] public DialogNode outputNode;
    [HideInInspector] public DialogNode inputNode;

    [HideInInspector] public DialogData prevOutputDialog;
    [HideInInspector] public DialogData prevInputDialog;

    public DialogTreeGraphView graphView;
    #region Managing changes between dialogs
    public void UpdateDialogs()
    {
        if(isConnectionPossible && !graphView.isPopulating)
        {
            //remove previous dialog transitions
            UnlinkConnection();

            ConnectionChangeConditions = new DialogChangeCondition[0];
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
            outputNode.DialogData.PossibleNextDialogs.Add(inputNode.DialogData, new DialogChangeCondition[0]);

    }
    #endregion

    #region Managing changes in edge
    public void Update()
    {
        if (outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData].Length != ConnectionChangeConditions.Length)
        {
            UpdateNodeConditions();
            return;
        }
        for(int i = 0; i <  ConnectionChangeConditions.Length; i++)
        {
            if (ConnectionChangeConditions[i] != outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData][i])
                UpdateNodeConditions();
        }
    }
    #endregion

    void UpdateNodeConditions()
    {
        outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData] = ConnectionChangeConditions;
        Debug.Log("New PossibleNextDialogs: " + outputNode.DialogData.PossibleNextDialogs[inputNode.DialogData].Length);
        Debug.Log("New ChangeConditions: " + ConnectionChangeConditions.Length);

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